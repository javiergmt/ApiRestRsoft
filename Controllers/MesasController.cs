using ApiRestRs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
//using System.Data;
using System.Data.SqlClient;
using ApiRestRs.Authentication;

namespace ApiRestRs.Controllers
{

    [Route("[action]")]
    [ApiController]
    public class MesasController : ControllerBase
    {
        public string? con;
        public MesasController(IConfiguration configuration)
        {
            string HeaderBD = configuration.GetConnectionString("default");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
        }

        


        [HttpGet("{count}/{idsector}")]
        [ActionName("mesas")]
        [EnableCors("MyCors")]

        public IEnumerable<Mesas> Mesas(IConfiguration configuration, int count, int idsector)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<Mesas> mesas = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Mesas", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@count", count);
                    cmd.Parameters.AddWithValue("@idsector", idsector);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mesas m = new Mesas
                            {
                                NroMesa = Convert.ToInt32(reader["NroMesa"]),
                                IdSector = Convert.ToInt32(reader["IdSector"]),
                                PosTop = Convert.ToInt32(reader["PosTop"]),
                                PosLeft = Convert.ToInt32(reader["PosLeft"]),
                                Width = Convert.ToInt32(reader["Width"]),
                                Height = Convert.ToInt32(reader["Height"]),
                                Ocupada = Convert.ToChar(reader["Ocupada"]),
                                Nro2 = reader["Nro2"] as int? ?? null,
                                IdMozo = reader["IdMozo"] as int? ?? 0,
                                Forma = Convert.ToInt32(reader["Forma"]),
                                Cerrada = Convert.ToInt32(reader["Cerrada"]),
                                CantSillas = reader["CantSillas"] as int? ?? 0,
                                CantPersonas = reader["CantPersonas"] as int? ?? 0,
                                DescMesa = reader["DescMesa"].ToString(),
                                Activa = Convert.ToInt32(reader["Activa"]),
                                ConPedEnEspera = Convert.ToInt32(reader["ConPedEnEspera"]),
                                Comiendo = Convert.ToInt32(reader["Comiendo"]),
                                Pendiente = Convert.ToInt32(reader["Pendiente"]),
                                PorCobrar = Convert.ToInt32(reader["PorCobrar"]),
                                Reservada = Convert.ToInt32(reader["Reservada"]),
                                SoloOcupada = Convert.ToInt32(reader["SoloOcupada"]),
                                ConPostre = Convert.ToInt32(reader["ConPostre"])

                            };
                            mesas.Add(m);

                        }
                    }
                    connection.Close();
                }

            }
            return mesas;

        }


        [HttpGet("{nromesa}/{sucursal}")]
        [ActionName("mesa")]
        [EnableCors("MyCors")]
        public IEnumerable<Mesa> Mesa(IConfiguration configuration, int nromesa, int sucursal)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<Mesa> mesa = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Mesa", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", nromesa);
                    cmd.Parameters.AddWithValue("@Sucursal", sucursal);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mesa m = new Mesa
                            {
                                Ocupada = Convert.ToChar(reader["Ocupada"]),
                                IdMozo = Convert.ToInt32(reader["IdMozo"]),
                                Cerrada = Convert.ToInt32(reader["Cerrada"]),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                CantPersonas = Convert.ToInt32(reader["CantPersonas"]),
                                Activa = Convert.ToInt32(reader["Activa"]),
                                PorCobrar = Convert.ToInt32(reader["PorCobrar"]),
                                Mozo = reader["Mozo"].ToString(),
                                Reservada = Convert.ToInt32(reader["Reservada"]),
                                NombreReserva = reader["NombreReserva"].ToString(),
                                IdReserva = reader["IdReserva"] as int? ?? 0,

                            };
                            mesa.Add(m);

                        }
                    }
                }
                connection.Close();
            }
            return mesa;

        }

        [HttpGet("{count}/{sucursal}/{delivery}")]
        [ActionName("sectores")]
        [EnableCors("MyCors")]
        public IEnumerable<Sectores> Sectores(IConfiguration configuration, int count, int sucursal, int delivery)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<Sectores> sectores = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Sectores", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@count", count);
                    cmd.Parameters.AddWithValue("@Sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@iddelivery", delivery);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Sectores s = new Sectores
                            {
                                IdSector = Convert.ToInt32(reader["idSector"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Comensales = reader["Comensales"] as int? ?? 0

                            };
                            sectores.Add(s);

                        }
                    }
                }
                connection.Close();
            }
            return sectores;
        }

        [HttpGet("{idmozo}")]
        [ActionName("mesas_mozos")]
        [EnableCors("MyCors")]
        public IEnumerable<MesasMozos> MesasMozos(IConfiguration configuration, int idmozo)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<MesasMozos> mesasmozos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_MesasMozos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idMozo", idmozo);
                 
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MesasMozos m = new MesasMozos
                            {
                                NroMesa = Convert.ToInt32(reader["NroMesa"]),
                                Cerrada = Convert.ToInt32(reader["Cerrada"]),
                                Sector = Convert.ToInt32(reader["Sector"]),
                                Importe = Convert.ToDecimal(reader["Importe"]),
                                Descripcion = reader["Descripcion"].ToString()
                                

                            };
                            mesasmozos.Add(m);

                        }
                    }
                }
                connection.Close();
            }
            return mesasmozos;
        }


        [HttpGet("{nromesa}")]
        [ActionName("mesa_enc")]
        [EnableCors("MyCors")]
        public IEnumerable<MesaEnc> MesaEnc(IConfiguration configuration, int nromesa)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<MesaEnc> mesaenc = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_MesaEnc", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", nromesa);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MesaEnc m = new MesaEnc
                            {
                                nroMesa = Convert.ToInt32(reader["NroMesa"]),
                                idMozo = Convert.ToInt32(reader["IdMozo"]),                                
                                fecha = Convert.ToDateTime(reader["Fecha"]),
                                cerrada = Convert.ToInt32(reader["Cerrada"]),
                                porcDesc = Convert.ToDecimal(reader["PorcDesc"]),
                                cantPersonas = Convert.ToInt32(reader["CantPersonas"]),
                                descPesos = Convert.ToDecimal(reader["DescPesos"]),
                                fechaHoraImp = (reader["FechaHoraImp"] == DBNull.Value) ? default(DateTime?) : Convert.ToDateTime(reader["FechaHoraImp"]),
                                idCliente = reader["IdCliente"] as int? ?? 0,
                                idOcupacion = Convert.ToInt32(reader["IdOcupacion"]),
                                idSector = Convert.ToInt32(reader["IdSector"]),
                                descMesa = reader["DescMesa"].ToString(),
                                nombre = reader["Nombre"].ToString()

                            };
                            mesaenc.Add(m);

                        }
                    }
                }
                connection.Close();
            }
            return mesaenc;

        }


        [HttpGet("{nromesa}/{agrupar}/{idPedido}")]
        [ActionName("mesa_det")]
        [EnableCors("MyCors")]
        public IEnumerable<MesaDet> MesaDet(IConfiguration configuration, int nromesa, int agrupar, int idPedido)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<MesaDet> mesadet = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_MesaDet", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", nromesa);
                    cmd.Parameters.AddWithValue("@Agrupar", agrupar);
                    cmd.Parameters.AddWithValue("@idPedido", idPedido);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MesaDet m = new MesaDet
                            {
                                NroMesa = Convert.ToInt32(reader["NroMesa"]),
                                IdDetalle = Convert.ToInt32(reader["IdDetalle"]),
                                IdPlato = Convert.ToInt32(reader["IdPlato"]),
                                Cant = Convert.ToDecimal(reader["Cant"]),
                                PcioUnit = Convert.ToDecimal(reader["PcioUnit"]),
                                Importe = Convert.ToDecimal(reader["Importe"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Cocido = Convert.ToChar(reader["Cocido"]),
                                idTipoConsumo = reader["idTipoconsumo"].ToString()
                            };
                            mesadet.Add(m);

                        }
                    }
                }
                connection.Close();
            }
            return mesadet;

        }

        [HttpGet("{nromesa}")]
        [ActionName("mesa_pagos")]
        [EnableCors("MyCors")]
        public IEnumerable<MesaPagos> MesaPagos(IConfiguration configuration, int nromesa)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<MesaPagos> mesapagos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_MesaPagos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", nromesa);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MesaPagos m = new MesaPagos
                            {
                                  Pagos = Convert.ToInt32(reader["Pagos"])
                            };
                          
                           
                            mesapagos.Add(m);

                        }
                    }
                }
                connection.Close();
            }
            return mesapagos;
        }

        [HttpGet("")]
        [ActionName("lugSectImpre")]
        [EnableCors("MyCors")]
        public IEnumerable<LugSectImpre> LugSectImpre(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<LugSectImpre> lugSect = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_LugSectImpre", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LugSectImpre s = new LugSectImpre
                            {
                                idLugarExped = Convert.ToInt32(reader["idLugarExped"]),
                                idSectorExped = Convert.ToInt32(reader["idSectorExped"]),
                                idImpresora = Convert.ToInt32(reader["idImpresora"]),
                                descripcion = reader["descripcion"].ToString()

                            };

                            lugSect.Add(s);

                        }
                    }
                }
                connection.Close();
            }
            return lugSect;
        }


        [HttpGet("")]
        [ActionName("mesa_formas")]
        [EnableCors("MyCors")]
        public IEnumerable<MesaForma> MesaForma(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<MesaForma> mesa = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_MesaFormas", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MesaForma m = new MesaForma
                            {
                                idForma = Convert.ToInt32(reader["idForma"]),
                                descripcion = reader["descripcion"].ToString()

                            };

                            mesa.Add(m);

                        }
                    }
                }
                connection.Close();
            }
            return mesa;
        }


        [HttpGet("")]
        [ActionName("mesas_objetos_plano")]
        [EnableCors("MyCors")]
        public IEnumerable<MesasObjetos> MesasObjetos(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<MesasObjetos> objetos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_MesasObjetosPlano", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MesasObjetos m = new MesasObjetos
                            {
                                idObjeto = Convert.ToInt32(reader["idObjeto"]),
                                descripcion = reader["descripcion"].ToString(),
                                forma = Convert.ToInt32(reader["forma"]),
                                idSector = Convert.ToInt32(reader["idSector"]),
                                color = reader["color"].ToString(),
                                penColor = reader["penColor"].ToString(),
                                brushStyle = Convert.ToInt32(reader["brushStyle"]),
                                penStyle = Convert.ToInt32(reader["penStyle"]),
                                posTop = Convert.ToInt32(reader["posTop"]),
                                posLeft = Convert.ToInt32(reader["posLeft"]),
                                width = Convert.ToInt32(reader["width"]),
                                height = Convert.ToInt32(reader["height"]),
                                puntasRedondeadas = Convert.ToBoolean(reader["puntasRedondeadas"])

                            };

                            objetos.Add(m);

                        }
                    }
                }
                connection.Close();
            }
            return objetos;
        }
    }
}
