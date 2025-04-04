﻿//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ApiRestRs.Authentication;
//using static System.Net.Mime.MediaTypeNames;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        public string? con;
        public PedidosController(IConfiguration configuration)
        {
            string HeaderBD = configuration.GetConnectionString("default");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
        }

        [HttpGet("{idRepartidor}/{fechaDesde}/{fechaHasta}/{cobrado}/{noPedidosCerrados}/{ptoVta}")]
        [ActionName("pedidos")]
        [EnableCors("MyCors")]
        public IEnumerable<Pedidos> Pedidos(IConfiguration configuration, int idRepartidor, DateTime fechaDesde, DateTime fechaHasta, int cobrado, int noPedidosCerrados, int ptoVta)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<Pedidos> pedidos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Pedidos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idRepartidor", idRepartidor);
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta);
                    cmd.Parameters.AddWithValue("@cobrado", cobrado);
                    cmd.Parameters.AddWithValue("@noPedidosCerrados", noPedidosCerrados);
                    cmd.Parameters.AddWithValue("@ptoVta", ptoVta);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pedidos p = new Pedidos
                            {
                                ptoDeVta = Convert.ToInt32(reader["PuntoDeVenta"]),
                                idPedido = Convert.ToInt32(reader["idPedido"]),
                                CodClie = Convert.ToInt32(reader["CodClie"]),
                                Cliente = reader["cliente"].ToString(),
                                FechaEntrega = Convert.ToDateTime(reader["FechaEntrega"]),
                                HoraEntrega = reader["HoraEntrega"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                Total = Convert.ToDecimal(reader["Total"]),
                                Total1 = Convert.ToDecimal(reader["Total1"]),
                                Total2 = Convert.ToDecimal(reader["Total2"]),
                                sPagaCon = reader["sPagaCon"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Repartidor = reader["Repartidor"].ToString(),
                                Cobrado = Convert.ToInt32(reader["Cobrado"]),
                                xMostrador = Convert.ToInt32(reader["xMostrador"]),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                Hora = reader["Hora"].ToString(),
                                Usuario = reader["Usuario"].ToString(),
                                Minutos = Convert.ToDecimal(reader["Minutos"])


                            };
                            pedidos.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return pedidos;

        }

        [HttpGet("{idPedido}")]
        
        [ActionName("pedido_enc")]
        [EnableCors("MyCors")]
        public IEnumerable<PedEnc> PedEnc(IConfiguration configuration, int idPedido)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<PedEnc> pedido = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PedidoEnc", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPedido", idPedido);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PedEnc p = new PedEnc
                            {
                                idPedido = Convert.ToInt32(reader["idPedido"]),
                                fecha = Convert.ToDateTime(reader["fecha"]),
                                hora = reader["hora"].ToString(),
                                idCliente = Convert.ToInt32(reader["idCliente"]),
                                fechaEntrega = Convert.ToDateTime(reader["fechaEntrega"]),
                                horaEntrega = reader["horaEntrega"].ToString(),
                                subtotal = Convert.ToDecimal(reader["subtotal"]),
                                descuento = Convert.ToDecimal(reader["descuento"]),
                                total = Convert.ToDecimal(reader["total"]),
                                envio = Convert.ToDecimal(reader["envio"]),
                                pago = Convert.ToDecimal(reader["pago"]),
                                pagaCon = Convert.ToDecimal(reader["pagaCon"]),
                                obs = reader["obs"].ToString(),
                                idRepartidor = Convert.ToInt32(reader["idRepartidor"]),
                                nombreClie = reader["nombreClie"].ToString(),
                                direccionClie = reader["direccionClie"].ToString(),
                                enUso = Convert.ToBoolean(reader["enUso"]),
                                Cobrado = Convert.ToBoolean(reader["Cobrado"]),
                                xMostrador = Convert.ToBoolean(reader["xMostrador"]),
                                idUsuario = Convert.ToInt32(reader["idUsuario"]),
                                puntoDeVenta = Convert.ToInt32(reader["puntoDeVenta"]),
                                delivery = Convert.ToBoolean(reader["delivery"]),
                                tipoDesc = Convert.ToInt32(reader["tipoDesc"]),
                                descRec = Convert.ToDecimal(reader["descRec"])
                            };
                            
                            pedido.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return pedido;

        }



    } // Fin de la clase PedidosController
} // Fin del namespace ApiRestRs.Controllers

