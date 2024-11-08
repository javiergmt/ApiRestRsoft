using ApiRestRs.Authentication;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class PlatosController : ControllerBase 
    {
        
        public string? con;
        public PlatosController(IConfiguration configuration)
        {
            string HeaderBD = configuration.GetConnectionString("default");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
        }


        [HttpGet("{coper}/{ccadena}/{nrubro}/{nsubrubro}")]
        [ActionName("platos")]
        [EnableCors("MyCors")]
        public IEnumerable<Platos> Platos(IConfiguration configuration, string coper, string ccadena, int nrubro, int nsubrubro)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<Platos> platos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Platos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cOper", coper);
                    cmd.Parameters.AddWithValue("@cCadena", ccadena);
                    cmd.Parameters.AddWithValue("@nRubro", nrubro);
                    cmd.Parameters.AddWithValue("@nSubRubro", nsubrubro);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Platos p = new Platos
                            {
                                IdPlato = Convert.ToInt32(reader["IdPlato"]),
                                DescCorta = reader["DescCorta"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                pcioUnit = Convert.ToDecimal(reader["Precio"]),
                                TamanioUnico = Convert.ToBoolean(reader["TamanioUnico"]),
                                idTipoConsumo = reader["idTipoConsumo"].ToString(),
                                idRubro = Convert.ToInt32(reader["idRubro"]),
                                idSubRubro = Convert.ToInt32(reader["idSubRubro"]),
                                cantgustos = reader["cantgustos"] as int? ?? 0,
                                codBarra = reader["codBarra"].ToString(),
                                colorFondo = reader["colorFondo"].ToString(),
                                tecla = reader["tecla"] as int? ?? 0,
                                orden = reader["orden"] as int? ?? 0,
                                shortCut = reader["shortCut"].ToString(),
                                pedirCantAlCargar = Convert.ToBoolean(reader["pedirCantAlCargar"])


                            };
                            platos.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return platos;
        }

        [HttpGet("{idplato}")]
        [ActionName("platos_gustos")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatosGustos> PlatosGustos(IConfiguration configuration, int idplato)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<PlatosGustos> platosgustos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatosGustos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idplato", idplato);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatosGustos p = new PlatosGustos
                            {
                                IdPlato = Convert.ToInt32(reader["IdPlato"]),
                                IdGusto = Convert.ToInt32(reader["IdGusto"]),
                                DescGusto = reader["DescGusto"].ToString()

                            };
                            platosgustos.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return platosgustos;
        }

        [HttpGet("{idplato}/{idsector}")]
        [ActionName("platos_tamanios")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatosTamanios> PlatosTamanios(IConfiguration configuration, int idplato, int idsector)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<PlatosTamanios> platostam = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatosTamanios", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idplato", idplato);
                    cmd.Parameters.AddWithValue("@idsector", idsector);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatosTamanios p = new PlatosTamanios
                            {
                                IdPlato = Convert.ToInt32(reader["IdPlato"]),
                                IdTamanio = Convert.ToInt32(reader["IdTamanio"]),
                                DescTam = reader["DescTam"].ToString()

                            };
                            platostam.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return platostam;
        }

        [HttpGet("{idplato}/{idtam}/{idsector}/{hora}")]
        [ActionName("plato_precio")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoPrecio> PlatosPrecio(IConfiguration configuration, int idplato, int idtam, int idsector, string hora)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<PlatoPrecio> plato = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatoPrecio", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nplato", idplato);
                    cmd.Parameters.AddWithValue("@idtam", idtam);
                    cmd.Parameters.AddWithValue("@idsector", idsector);
                    cmd.Parameters.AddWithValue("@hora", hora);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatoPrecio p = new PlatoPrecio
                            {
                                IdPlato = Convert.ToInt32(reader["IdPlato"]),
                                pcioUnit = Convert.ToDecimal(reader["Precio"]),
                                IdSectorExped = Convert.ToInt32(reader["IdSectorExp"]),
                                impCentralizada = Convert.ToInt32(reader["impCentralizada"])

                            };
                            plato.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return plato;
        }

        [HttpGet("{idplato}")]
        [ActionName("plato_en_mesa")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoEnMesa> PlatoEnMesa(IConfiguration configuration, int idplato)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<PlatoEnMesa> plato = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatoEnMesa", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPlato", idplato);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatoEnMesa p = new PlatoEnMesa
                            {
                                Nro_Mesa = Convert.ToInt32(reader["Nro_Mesa"]),
                                Hora = reader["Hora"].ToString(),
                                Cant = Convert.ToDecimal(reader["Cant"]),
                                CodMozo = Convert.ToInt32(reader["CodMozo"]),
                                Mozo = reader["Mozo"].ToString(),
                                CargadoPor = reader["CargadoPor"].ToString()

                            };
                            plato.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return plato;
        }


        [HttpGet("{idplato}")]
        [ActionName("combo_sec")]
        [EnableCors("MyCors")]
        public IEnumerable<ComboSec> ComboSec(IConfiguration configuration, int idplato)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<ComboSec> combo = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_ComboSec", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idplato", idplato);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComboSec p = new ComboSec
                            {
                                idSeccion = Convert.ToInt32(reader["idSeccion"]),
                                idPlato = Convert.ToInt32(reader["idPlato"]),
                                descripcion = reader["Descripcion"].ToString(),
                                cantMax = Convert.ToDecimal(reader["CantMax"]),
                                orden = Convert.ToInt32(reader["Orden"]),
                                autocompletar = Convert.ToBoolean(reader["Autocompletar"]),
                                imprimirEnAceptacion = Convert.ToBoolean(reader["ImprimirEnAceptacion"]),
                                seleccionarUno = Convert.ToBoolean(reader["SeleccionarUno"]),
                                idTamanio = Convert.ToInt32(reader["IdTamanio"]),
                                descCorta = reader["DescCorta"].ToString(),
                                tamanio = reader["Tamanio"].ToString(),
                                platoSel = Convert.ToInt32(reader["PlatoSel"]),
                                idTipoConsumo = reader["idTipoConsumo"].ToString()

                            };
                            combo.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return combo;
        }


        [HttpGet("{idseccion}")]
        [ActionName("combo_det")]
        [EnableCors("MyCors")]
        public IEnumerable<ComboDet> ComboDet(IConfiguration configuration, int idseccion)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<ComboDet> combo = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_ComboDet", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idseccion", idseccion);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComboDet p = new ComboDet
                            {
                                idSeccion = Convert.ToInt32(reader["idSeccion"]),
                                idPlato = Convert.ToInt32(reader["idPlato"]),
                                idTamanio = Convert.ToInt32(reader["IdTamanio"]),
                                descCorta = reader["DescCorta"].ToString(),
                                descripcion = reader["Descripcion"].ToString(),
                                idTipoConsumo = reader["idTipoConsumo"].ToString(),
                                cantGustos = Convert.ToInt32(reader["CantGustos"]),
                                tamanio = reader["Tamanio"].ToString(),
                                idSectorExped = Convert.ToInt32(reader["IdSectorExped"]),
                                impCentralizada = Convert.ToInt32(reader["ImprimeEncomandaCentralizada"])
                            };
                            combo.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return combo;
        }


        [HttpGet("")]
        [ActionName("platos_obs")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatosObs> PlatosObs(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<PlatosObs> obs = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatosObs", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatosObs p = new PlatosObs
                            {
                                idObs = Convert.ToInt32(reader["IdObs"]),
                                descripcion = reader["Descripcion"].ToString()

                            };
                            obs.Add(p);

                        }
                    }
                }
                connection.Close();

            }
            return obs;
        }

        [HttpGet("{nroMesa}/{idDetalle}/{idPedido}")]
        [ActionName("plato_info")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoInfo> PlatoInfo(IConfiguration configuration, int nroMesa, int idDetalle, int idPedido)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            List<PlatoInfo> plato = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatoInfo", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", idDetalle);
                    cmd.Parameters.AddWithValue("@idPedido", idPedido);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatoInfo p = new PlatoInfo
                            {
                                idPlato = Convert.ToInt32(reader["idPlato"]),
                                descripcion = reader["Descripcion"].ToString(),
                                cant = Convert.ToDecimal(reader["Cant"]),
                                tamanio = reader["Tamanio"].ToString(),
                                hora = reader["Hora"].ToString(),
                                mozo = reader["Mozo"].ToString(),
                                usuario = reader["Usuario"].ToString(),
                                gustos = Convert.ToInt32(reader["Gustos"]),
                                idTipoConsumo = reader["idTipoConsumo"].ToString(),
                                obs = reader["Obs"].ToString()

                            };
                            plato.Add(p);

                        }
                    }
                }
                connection.Close();

            }
            return plato;

        }

        [HttpGet("{nroMesa}/{idDetalle}/{idPedido}")]
        [ActionName("plato_info_gustos")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoInfoGustos> PlatoInfoGustos(IConfiguration configuration, int nroMesa, int idDetalle, int idPedido)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<PlatoInfoGustos> plato = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatoInfoGustos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", idDetalle);
                    cmd.Parameters.AddWithValue("@idPedido", idPedido);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatoInfoGustos p = new PlatoInfoGustos
                            {
                                idGusto = Convert.ToInt32(reader["idGusto"]),
                                descripcion = reader["Descripcion"].ToString()
                            };
                            plato.Add(p);

                        }
                    }
                }
                connection.Close();

            }
            return plato;

        }

        [HttpGet("{nroMesa}/{idDetalle}/{idPedido}")]
        [ActionName("plato_info_combo")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoInfoCombo> PlatoInfoCombo(IConfiguration configuration, int nroMesa, int idDetalle, int idPedido)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<PlatoInfoCombo> plato = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatoInfoCombo", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", idDetalle);
                    cmd.Parameters.AddWithValue("@idPedido", idPedido);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatoInfoCombo p = new PlatoInfoCombo
                            {
                                idPlato = Convert.ToInt32(reader["idPlato"]),
                                cant = Convert.ToDecimal(reader["Cant"]),
                                descripcion = reader["Descripcion"].ToString(),
                                idSeccion = Convert.ToInt32(reader["idSeccion"]),
                                idTamanio = Convert.ToInt32(reader["IdTamanio"]),
                                cantGustos = Convert.ToInt32(reader["CantGustos"]),
                                idTipoConsumo = reader["idTipoConsumo"].ToString()
                            };
                            plato.Add(p);

                        }
                    }
                }
                connection.Close();

            }
            return plato;


        }

        [HttpGet("{nroMesa}/{idDetalle}/{idPedido}")]
        [ActionName("plato_info_combo_gustos")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoInfoComboGustos> PlatoInfoComboGustos(IConfiguration configuration, int nroMesa, int idDetalle, int idPedido)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<PlatoInfoComboGustos> plato = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatoInfoComboGustos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", idDetalle);
                    cmd.Parameters.AddWithValue("@idPedido", idPedido);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatoInfoComboGustos p = new PlatoInfoComboGustos
                            {
                                idPlato = Convert.ToInt32(reader["idPlato"]),
                                descGusto = reader["DescGusto"].ToString(),
                                idSeccion = Convert.ToInt32(reader["idSeccion"]),
                                idGusto = Convert.ToInt32(reader["IdGusto"])

                            };
                            plato.Add(p);

                        }
                    }
                }
                connection.Close();

            }
            return plato;
        }

        [HttpGet("")]
        [ActionName("obsrenglones")]
        [EnableCors("MyCors")]
        public IEnumerable<ObsRenglones> ObsRenglones(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<ObsRenglones> obs = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_ObsRenglones", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ObsRenglones o = new ObsRenglones
                            {
                                idObs = Convert.ToInt32(reader["idObs"]),
                                descripcion = reader["Descripcion"].ToString(),
                                
                            };
                            obs.Add(o);

                        }
                    }
                }
                connection.Close();

            }
            return obs;
        }
    }

}
