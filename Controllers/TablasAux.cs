
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;



namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class TablasAuxController : ControllerBase
    {
       
        public readonly string? con;
        public TablasAuxController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("conexion") + " Password=6736";
        }
        
        [HttpGet("")]
        [ActionName("clientes")]
        [EnableCors("MyCors")]
        public IEnumerable<Clientes> Clientes()
        {
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
                                porcDesc = reader["porcDesc"] as int? ?? 0,
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
        public IEnumerable<FormasDePago> FormasDePago()
        {
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
        public IEnumerable<CondicionIva> CondicionIva()
        {
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
        public IEnumerable<ZonasClientes> ZonasClientes()
        {
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
        public IEnumerable<Repartidores> Repartidores()
        {
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

    } // Fin de la clase TablasAuxController
} // fin del namespace ApiRestRs.Controllers
