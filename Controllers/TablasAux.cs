
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using ApiRestRs.Authentication;



namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class TablasAuxController : ControllerBase
    {
       
        public  string? con;
        public TablasAuxController(IConfiguration configuration)
        {

            string HeaderBD = configuration.GetConnectionString("default");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
            // leer de appsettings.json, puede ser util si se pasa un parametro
            // y en appsettings.json se cambia el valor de la conexion
            //con = configuration.GetValue<string>("ConnectionStrings:conexion") + " Password=6736"; 
        }
        
        [HttpGet("")]
        [ActionName("clientes")]
        [EnableCors("MyCors")]
        public IEnumerable<Clientes> Clientes(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<Clientes> clientes = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Clientes", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Clientes c = new Clientes
                            {
                                IdCliente = Convert.ToInt32(reader["idCliente"]),
                                nombre = reader["nombre"].ToString(),
                                direccion = reader["direccion"].ToString(),
                                telefono = reader["telefono"].ToString(),
                                email = reader["email"].ToString(),
                                telefono2 = reader["telefono2"].ToString(),
                                telefono3 = reader["telefono3"].ToString(),
                                idCondIva = reader["idCondIva"].ToString(),
                                cuit = reader["cuit"].ToString(),
                                localidad = reader["localidad"].ToString(),
                                codigoPostal = reader["codigoPostal"].ToString(),
                                idZona = reader["idZona"] as int? ?? 0,
                                fechaNac = Convert.ToDateTime(reader["fechaNac"]),
                                obs = reader["obs"].ToString(),
                                credito = reader["credito"] as int? ?? 0,
                                bloquearCredito = Convert.ToBoolean(reader["bloquearCredito"]),
                                porcDesc = reader["porcDesc"] as decimal? ?? 0,
                                aCtaCte = Convert.ToBoolean(reader["aCtaCte"]),
                                idTarjeta = reader["idTarjeta"].ToString(),
                                activo = Convert.ToBoolean(reader["activo"]),
                                nombreFantasia = reader["nombreFantasia"].ToString(),
                                perIbMinimo = reader["perIbMinimo"] as decimal? ?? 0,
                                perIbAlicuota = reader["perIbAlicuota"] as decimal? ?? 0,
                                perIbTipo = reader["perIbTipo"].ToString(),
                                autFactA = Convert.ToBoolean(reader["autFactA"]),
                                vtoform8001 = Convert.ToDateTime(reader["vtoform8001"])

                            };
                            clientes.Add(c);

                        }
                    }
                }
                connection.Close();

            }
            return clientes;

        }

        [HttpGet("")]
        [ActionName("formas_pago")]
        [EnableCors("MyCors")]
        public IEnumerable<FormasDePago> FormasDePago(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<FormasDePago> fpago = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_FormasDePago", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FormasDePago f = new FormasDePago
                            {
                                idForma = Convert.ToInt32(reader["idForma"]),
                                id = Convert.ToInt32(reader["id"]),
                                forma = reader["forma"].ToString(),
                                orden = Convert.ToInt32(reader["orden"]),
                                

                            };
                            fpago.Add(f);

                        }
                    }
                }
                connection.Close();

            }
            return fpago;

        }

        [HttpGet("")]
        [ActionName("condicion_iva")]
        [EnableCors("MyCors")]
        public IEnumerable<CondicionIva> CondicionIva(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<CondicionIva> civa = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_CondicionIva", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CondicionIva c = new CondicionIva
                            {
                                idCondicionIva = reader["idCondIVA"].ToString(),
                                descripcion = reader["Descripcion"].ToString(),
                                letraFact = reader["LetraFact"].ToString(),
                                
                            };
                            civa.Add(c);

                        }
                    }
                }
                connection.Close();

            }
            return civa;

        }

        [HttpGet("")]
        [ActionName("zonas_clientes")]
        [EnableCors("MyCors")]
        public IEnumerable<ZonasClientes> ZonasClientes(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<ZonasClientes> zonas = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_ZonasClientes", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ZonasClientes c = new ZonasClientes
                            {
                                idZona = Convert.ToInt32(reader["idZona"]),
                                descripcion = reader["Descripcion"].ToString(),

                            };
                              
                            zonas.Add(c);

                        }
                    }
                }
                connection.Close();

            }
            return zonas;

        }

        [HttpGet("")]
        [ActionName("repartidores")]
        [EnableCors("MyCors")]
        public IEnumerable<Repartidores> Repartidores(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<Repartidores> repart = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Repartidores", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Repartidores r = new Repartidores
                            {
                                idRepartidor = Convert.ToInt32(reader["idRepartidor"]),
                                nombre = reader["Nombre"].ToString(),

                            };

                            repart.Add(r);

                        }
                    }
                }
                connection.Close();

            }
            return repart;

        }

        [HttpGet("")]
        [ActionName("obsdescuentos")]
        [EnableCors("MyCors")]
        public IEnumerable<ObsDescuentos> ObsDescuentos(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<ObsDescuentos> obs = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_ObsDescuentos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ObsDescuentos r = new ObsDescuentos
                            {
                                idObs = Convert.ToInt32(reader["idObs"]),
                                descripcion = reader["Descripcion"].ToString(),

                            };

                            obs.Add(r);

                        }
                    }
                }
                connection.Close();

            }
            return obs;

        }

        [HttpGet("")]
        [ActionName("turnos")]
        [EnableCors("MyCors")]
        public IEnumerable<Turnos> Turnos(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<Turnos> turnos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Turnos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Turnos t = new Turnos
                            {
                                idTurno = Convert.ToInt32(reader["idTurno"]),
                                descripcion = reader["Descripcion"].ToString(),
                                horaDesde = reader["horaDesde"].ToString(),
                                horaHasta = reader["horaHasta"].ToString()
                            };

                            turnos.Add(t);

                        }
                    }
                }
                connection.Close();

            }
            return turnos;

        }


        [HttpGet("")]
        [ActionName("param_delivery")]
        [EnableCors("MyCors")]
        public IEnumerable<ParamDelivery> ParamDelivery(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<ParamDelivery> param = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_ParamDelivery", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ParamDelivery p = new ParamDelivery
                            {
                                MaxPorcDesc = Convert.ToDecimal(reader["maxPorcDesc"]),
                                EnvioDelivery = Convert.ToDecimal(reader["envioDelivery"]),
                                DeliveryMostradorFacturar = Convert.ToBoolean(reader["deliveryMostradorFacturar"]),
                                DeliveryClientesFacturar = Convert.ToBoolean(reader["deliveryClientesFacturar"]),
                                DeliveryImpAlGuardar = Convert.ToBoolean(reader["deliveryImpAlGuardar"]),
                                DeliverySoloPlatosDelivery = Convert.ToBoolean(reader["deliverySoloPlatosDelivery"]),
                                MesasNoPlatosDelivery = Convert.ToBoolean(reader["mesasNoPlatosDelivery"]),
                                DeliveryDemorado = Convert.ToInt32(reader["deliveryDemorado"]),
                                DeliveryMuyDemorado = Convert.ToInt32(reader["deliveryMuyDemorado"]),
                                DeliveryColorDemorado = reader["deliveryColorDemorado"].ToString(),
                                DeliveryColorMuyDemorado = reader["deliveryColorMuyDemorado"].ToString(),
                                PorcDescPagoEfectivo = Convert.ToDecimal(reader["porcDescPagoEfectivo"]),
                                idMotivoDescPagoEfectivo = Convert.ToInt32(reader["idMotivoDescPagoEfectivo"]),
                                AgruparPlatosIguales = Convert.ToBoolean(reader["agruparPlatosIguales"]),
                                MostrarResumenDelivery = Convert.ToBoolean(reader["mostrarResumenDelivery"])


                            };

                            param.Add(p);

                        }
                    }
                }
                connection.Close();

            }
            return param;

        }

    } // Fin de la clase TablasAuxController
} // fin del namespace ApiRestRs.Controllers
