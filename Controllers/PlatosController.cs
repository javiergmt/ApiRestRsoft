using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class PlatosController
    {
        public readonly string? con;
        public PlatosController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("conexion") + " Password=6736";
        }


        [HttpGet("{coper}/{ccadena}/{nrubro}/{nsubrubro}")]
        [ActionName("platos")]
        [EnableCors("MyCors")]
        public IEnumerable<Platos> Platos(string coper, string ccadena, int nrubro, int nsubrubro)
        {
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

            }
            return platos;
        }

        [HttpGet("{idplato}")]
        [ActionName("platos_gustos")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatosGustos> PlatosGustos(int idplato)
        {
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

            }
            return platosgustos;
        }

        [HttpGet("{idplato}/{idsector}")]
        [ActionName("platos_tamanios")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatosTamanios> PlatosTamanios(int idplato, int idsector)
        {
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

            }
            return platostam;
        }

        [HttpGet("{idplato}/{idtam}/{idsector}/{hora}")]
        [ActionName("plato_precio")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoPrecio> PlatosPrecio(int idplato, int idtam, int idsector, string hora)
        {
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
                                IdSectorExped = Convert.ToInt32(reader["IdSectorExp"])

                            };
                            plato.Add(p);

                        }
                    }
                }

            }
            return plato;
        }

        [HttpGet("{idplato}")]
        [ActionName("plato_en_mesa")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoEnMesa> PlatoEnMesa(int idplato)
        {
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

            }
            return plato;
        }


        [HttpGet("{idplato}")]
        [ActionName("combo_sec")]
        [EnableCors("MyCors")]
        public IEnumerable<ComboSec> ComboSec(int idplato)
        {
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

            }
            return combo;
        }


        [HttpGet("{idseccion}")]
        [ActionName("combo_det")]
        [EnableCors("MyCors")]
        public IEnumerable<ComboDet> ComboDet(int idseccion)
        {
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
                                tamanio = reader["Tamanio"].ToString()
                            };
                            combo.Add(p);

                        }
                    }
                }

            }
            return combo;
        }


        [HttpGet("")]
        [ActionName("platos_obs")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatosObs> PlatosObs()
        {
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

            }
            return obs;
        }

        [HttpGet("{nroMesa}/{idDetalle}")]
        [ActionName("plato_info")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoInfo> PlatoInfo(int nroMesa, int idDetalle)
        {
            List<PlatoInfo> plato = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatoInfo", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", idDetalle);

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

            }
            return plato;

        }

        [HttpGet("{nroMesa}/{idDetalle}")]
        [ActionName("plato_info_gustos")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoInfoGustos> PlatoInfoGustos(int nroMesa, int idDetalle)
        {
            List<PlatoInfoGustos> plato = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatoInfoGustos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", idDetalle);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatoInfoGustos p = new PlatoInfoGustos
                            {
                                descripcion = reader["Descripcion"].ToString()
                            };
                            plato.Add(p);

                        }
                    }
                }

            }
            return plato;

        }

        [HttpGet("{nroMesa}/{idDetalle}")]
        [ActionName("plato_info_combo")]
        [EnableCors("MyCors")]
        public IEnumerable<PlatoInfoCombo> PlatoInfoCombo(int nroMesa, int idDetalle)
        {
            List<PlatoInfoCombo> plato = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_PlatoInfoCombo", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", idDetalle);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlatoInfoCombo p = new PlatoInfoCombo
                            {
                                idPlato = Convert.ToInt32(reader["idPlato"]),
                                cant = Convert.ToDecimal(reader["Cant"]),
                                descripcion = reader["Descripcion"].ToString()
                            };
                            plato.Add(p);

                        }
                    }
                }

            }
            return plato;

        }
    }

}
