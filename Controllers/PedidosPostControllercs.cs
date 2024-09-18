using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using ApiRestRs.Authentication;

namespace ApiRestRs.Controllers
{
    
    [ApiController]
    public class PedidosPostController : ControllerBase

    {

        public string? con;
        public PedidosPostController(IConfiguration configuration)
        {
            string HeaderBD = configuration.GetConnectionString("default");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
        }

        [HttpPost]
        [Route("pedido_nuevo")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] PedEnc p)

        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_PedidoNuevo", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPedido", p.idPedido);
                    cmd.Parameters.AddWithValue("@fecha", p.fecha);
                    cmd.Parameters.AddWithValue("@hora", p.hora);
                    if (p.idCliente == 0)
                    {
                        cmd.Parameters.AddWithValue("@idCliente", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@idCliente", p.idCliente);
                    }

                    cmd.Parameters.AddWithValue("@fechaEntrega", p.fechaEntrega);
                    cmd.Parameters.AddWithValue("@horaEntrega", p.horaEntrega);
                    cmd.Parameters.AddWithValue("@subtotal", p.subtotal);
                    cmd.Parameters.AddWithValue("@descuento", p.descuento);
                    cmd.Parameters.AddWithValue("@total", p.total);
                    cmd.Parameters.AddWithValue("@envio", p.envio);
                    cmd.Parameters.AddWithValue("@pago", p.pago);
                    cmd.Parameters.AddWithValue("@pagaCon", p.pagaCon);
                    cmd.Parameters.AddWithValue("@obs", p.obs);
                    if (p.idRepartidor == 0)
                    {
                        cmd.Parameters.AddWithValue("@idRepartidor", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@idRepartidor", p.idRepartidor);
                    }

                    cmd.Parameters.AddWithValue("@nombreClie", p.nombreClie);
                    cmd.Parameters.AddWithValue("@direccionClie", p.direccionClie);
                    cmd.Parameters.AddWithValue("@enUso", p.enUso);
                    cmd.Parameters.AddWithValue("@Cobrado", p.Cobrado);
                    cmd.Parameters.AddWithValue("@xMostrador", p.xMostrador);
                    cmd.Parameters.AddWithValue("@idUsuario", p.idUsuario);
                    cmd.Parameters.AddWithValue("@puntoDeVenta", p.puntoDeVenta);
                    cmd.Parameters.AddWithValue("@delivery", p.delivery);
                    cmd.Parameters.AddWithValue("@tipoDesc", p.tipoDesc);
                    cmd.Parameters.AddWithValue("@descRec", p.descRec);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        int ultPedido = 0;
                        if (p.idPedido == 0)
                        {
                            SqlConnection conPed = new(con);
                            SqlDataReader dataPed;

                            string sqlPed = "select MAX(idPedido) as idPedido from PEDENC ";
                            conPed.Open();
                            SqlCommand cmdPed = new SqlCommand(sqlPed, conPed);
                            dataPed = cmdPed.ExecuteReader();
                            dataPed.Read();
                            try
                            {
                                ultPedido = Convert.ToInt32(dataPed["idPedido"]);

                            }
                            catch (Exception e)
                            {
                                ultPedido = 0;
                            }

                            conPed.Close();
                        }
                        else
                        {
                            ultPedido = p.idPedido;
                        }
                        return new JsonResult(new { res = "OK", pedido = ultPedido });
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return new JsonResult(new { res = ex, pedido = 0 });
                    }


                }
            }
        }

        [HttpPost]
        [Route("factura_crear")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] FacturaCrear f)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_FacturaCrear", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPedido", f.idPedido);
                    cmd.Parameters.AddWithValue("@nroMesa", f.nroMesa);
                    cmd.Parameters.AddWithValue("@pagoEnMesa", f.pagoEnMesa);
                    cmd.Parameters.AddWithValue("@fiscal", f.fiscal);
                    cmd.Parameters.AddWithValue("@idRepartidor", f.idRepartidor);
                    cmd.Parameters.AddWithValue("@idObsDesc", f.idObsDesc);
                    cmd.Parameters.AddWithValue("@idUsuario", f.idUsuario);
                    cmd.Parameters.AddWithValue("@idCliente", f.idCliente);
                    cmd.Parameters.AddWithValue("@total", f.total);
                    cmd.Parameters.AddWithValue("@tipoDesc", f.tipoDesc);
                    cmd.Parameters.AddWithValue("@impDesc", f.impDesc);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        string ultNro = "";
                        SqlConnection conPed = new(con);
                        SqlDataReader dataPed;
                        // Traigo el ultimo nro de factura
                        string sqlPed = "Select MAX(nro) as Nro From FACENC ";
                        conPed.Open();
                        SqlCommand cmdPed = new SqlCommand(sqlPed, conPed);
                        dataPed = cmdPed.ExecuteReader();
                        dataPed.Read();
                        try
                        {
                            ultNro = (dataPed["Nro"].ToString());

                        }
                        catch (Exception e)
                        {
                            ultNro = "";
                        }

                        conPed.Close();
                        return new JsonResult(new { res = "OK", nro = ultNro });
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return new JsonResult(new { res = ex, nro = 0 });
                    }


                }
            }
        }

        [HttpPost]
        [Route("factura_pagar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] FacturaPagar f)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_FacturaPagar", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nro", f.nro);
                    cmd.Parameters.AddWithValue("@importe", f.importe);
                    cmd.Parameters.AddWithValue("@idCliente", f.idCliente);
                    cmd.Parameters.AddWithValue("@idFormaPago", f.idFormaPago);
                    cmd.Parameters.AddWithValue("@idCupon", f.idCupon);
                    cmd.Parameters.AddWithValue("@idMoneda", f.idMoneda);
                    cmd.Parameters.AddWithValue("@importeMoneda", f.importeMoneda);
                    cmd.Parameters.AddWithValue("@cotizacion", f.cotizacion);
                    cmd.Parameters.AddWithValue("@billetes", f.billetes);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        return new JsonResult(new { res = "OK" });
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return new JsonResult(new { res = ex });
                    }
                }
            }
        }

        [HttpPost]
        [Route("pedido_renglon_cambiar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] PedidoRenglonCambiar p)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_PedidoRenglonCambiar", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPedido", p.idPedido);
                    cmd.Parameters.AddWithValue("@idDetalle", p.idDetalle);
                    cmd.Parameters.AddWithValue("@cant", p.cant);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        return new JsonResult(new { res = "OK" });
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return new JsonResult(new { res = ex });
                    }
                }
            }
        }

        [HttpPost]
        [Route("cliente_cambiar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] ClientesPedido c)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_ClienteCambiar", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCliente", c.IdCliente);
                    cmd.Parameters.AddWithValue("@nombre", c.nombre);
                    cmd.Parameters.AddWithValue("@direccion", c.direccion);
                    cmd.Parameters.AddWithValue("@localidad", c.localidad);
                    cmd.Parameters.AddWithValue("@tel1", c.telefono);
                    cmd.Parameters.AddWithValue("@email", c.email);
                    cmd.Parameters.AddWithValue("@tel2", c.telefono2);
                    cmd.Parameters.AddWithValue("@tel3", c.telefono3);
                    cmd.Parameters.AddWithValue("@idZona", c.idZona);
                    cmd.Parameters.AddWithValue("@fechaNac", c.fechaNac);
                    cmd.Parameters.AddWithValue("@idIva", c.idCondIva);
                    cmd.Parameters.AddWithValue("@cuit", c.cuit);
                    cmd.Parameters.AddWithValue("@tarj", c.idTarjeta);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        return new JsonResult(new { res = "OK" });
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return new JsonResult(new { res = ex });
                    }
                }
            }
        }
    }
}
