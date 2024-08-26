using ApiRestRs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;



namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]

    
    public class MozosController : ControllerBase
    {
        public string? con;
        

        public MozosController(IConfiguration configuration)
        {
                                   
            con = configuration.GetConnectionString("conexion") + " Password=6736";
            
            
            //Console.WriteLine("Conectado a la base de datos");
            //string HeaderBD;
            //if (request.Headers != null)
            //{
            //    foreach (var item in request.Headers)
            //    {
            //        if (item.Key == "bd")
            //        {
            //            HeaderBD = item.Value.First();
            //            Console.WriteLine(HeaderBD);
            //        }
            //    }
            //}
            //else
            //{
            //    HeaderBD = "Restobar";
            //}

        }

        [HttpGet("")]
        [ActionName("usuarios")]
        [EnableCors("MyCors")]

        public IEnumerable<Usuarios> Usuarios()
       
        {
            //Request.Headers.TryGetValue("bd", out var bd);
            //if (bd.Count > 0)
            //{
            //    //con = con + " Database=" + bd[0];
            //    Console.WriteLine(bd);
            //}
            //con = configuration.GetConnectionString("conexion") + " Database=RestobarW; Password=6736";

            List<Usuarios> usuarios = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Usuarios", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuarios u = new Usuarios
                            {
                                idUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                nombre = reader["Nombre"].ToString(),
                                alias = reader["Alias"].ToString(),
                                password = reader["Password"].ToString(),
                                idGrupo = Convert.ToInt32(reader["IdGrupo"]),

                            };
                            usuarios.Add(u);

                        }
                    }
                }
                connection.Close();
            }
            return usuarios;

        }

        [HttpGet("{pass}")]
        [ActionName("mozos_pass")]
        [EnableCors("MyCors")]

        public IEnumerable<Mozos> Mozos(string pass)
        {
            List<Mozos> mozos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_MozosPass", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pass", pass);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mozos m = new Mozos
                            {
                                IdMozo = Convert.ToInt32(reader["IdMozo"]),
                                Nombre = reader["Nombre"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"]),
                                Password = reader["Password"].ToString(),
                                Tecla = reader["Tecla"].ToString(),
                                IdTipoMozo = Convert.ToInt32(reader["IdTipoMozo"]),
                                NroPager = reader["NroPager"].ToString(),
                                Orden = Convert.ToInt32(reader["Orden"])

                            };
                            mozos.Add(m);

                        }
                    }
                }
                connection.Close();
            }
            return mozos;

        }

        

        [HttpGet("")]
        [ActionName("param_mozos")]
        [EnableCors("MyCors")]
        //[Route("param_mozos")]
        public IEnumerable<ParamMozos> ParamMozos()
        {
           
            List<ParamMozos> parammozos = new();
          

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_ParamMozos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ParamMozos m = new ParamMozos
                            {
                                ColorMesaCerrada = reader["ColorMesaCerrada"].ToString(),
                                ColorMesaComiendo = reader["ColorMesaComiendo"].ToString(),
                                ColorMesaComiendoEsperando = reader["ColorMesaComiendoEsperando"].ToString(),
                                ColorMesaEperando = reader["ColorMesaEperando"].ToString(),
                                ColorMesaNormal = reader["ColorMesaNormal"].ToString(),
                                ColorMesaOcupada = reader["ColorMesaOcupada"].ToString(),
                                ColorMesaPorCobrar = reader["ColorMesaPorCobrar"].ToString(),
                                ColorMesaSinPed = reader["ColorMesaSinPed"].ToString(),
                                ColorPostre = reader["ColorPostre"].ToString(),
                                Sucursal = reader["Sucursal"] as int? ?? 0,
                                ActivarSubEstadosMesa = Convert.ToBoolean(reader["ActivarSubEstadosMesa"]),
                                ModificaPrecioEnMesa = Convert.ToBoolean(reader["ModificaPrecioEnMesa"]),
                                PermiteDescLibre = Convert.ToBoolean(reader["PermiteDescLibre"]),
                                PermitePrecioCero = Convert.ToBoolean(reader["PermitePrecioCero"]),
                                AutorizarElimiarPlatoEnMesa = Convert.ToBoolean(reader["AutorizarElimiarPlatoEnMesa"]),
                                MarcarMesaSinCobrar = Convert.ToBoolean(reader["MarcarMesaSinCobrar"]),
                                MozosCierranMesa = Convert.ToBoolean(reader["MozosCierranMesa"]),
                                MostrarRubroFavoritos = Convert.ToBoolean(reader["MostrarRubroFavoritos"]),
                                OcultarTotalMesaMozos = Convert.ToBoolean(reader["OcultarTotalMesaMozos"]),
                                PedirMotElimRenglon = Convert.ToBoolean(reader["PedirMotElimRenglon"]),
                                PedirCubiertos = Convert.ToBoolean(reader["PedirCubiertos"] ?? false),
                                idCubiertos = reader["idCubiertos"] as int? ?? 0,
                                idTurnoCubierto = reader["idTurnoCubierto"] as int? ?? 0,
                                Nombre = reader["Nombre"].ToString(),
                                idPagWeb = reader["idTurnoCubierto"] as int? ?? 0,
                                NombreWeb = reader["NombreWeb"].ToString(),
                                sector_ini = reader["sector_ini"] as int? ?? 0

                            };
                            parammozos.Add(m);

                        }
                    }
                }
                connection.Close();
            }
            //Imprimir.ImprimirComanda();
            return parammozos;

        }

        [HttpGet("{id}")]
        [ActionName("disp_valido")]
        [EnableCors("MyCors")]

        public IEnumerable<Disp> Disp(string id)
        {
            List<Disp> disp = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Disp_Valido", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Disp d = new Disp
                            {
                                valido = Convert.ToInt32(reader["valido"])
                            };
                            disp.Add(d);

                        }
                    }
                }
                connection.Close();
            }
            return disp;

        }

        [HttpGet("")]
        [ActionName("noticias_mozos")]
        [EnableCors("MyCors")]

        public IEnumerable<NoticiasMozos> NoticiasMozos()
        {
            List<NoticiasMozos> noticias = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_NoticiasMozos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NoticiasMozos n = new NoticiasMozos
                            {
                                idMozo = Convert.ToInt32(reader["idMozo"]),
                                descNoticia = reader["descNoticia"].ToString(),
                            };
                            noticias.Add(n);

                        }
                    }
                }
                connection.Close();
            }
            return noticias;

        }

        [HttpGet("{cadena}")]
        [ActionName("st_procs")]
        [EnableCors("MyCors")]

        public IEnumerable<StProcs> StProcs(string cadena )
        {
            List<StProcs> stprocs = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                var sqltext = "Select ROW_NUMBER() OVER (ORDER BY CAST(GETDATE() AS TIMESTAMP)) AS ID ," +
                    "ROUTINE_NAME as nombre, ROUTINE_DEFINITION as definicion " +
                    "from INFORMATION_SCHEMA.routines " +
                    "where (ROUTINE_TYPE = 'PROCEDURE') and ( SUBSTRING(ROUTINE_NAME,1,4)='spG_' or SUBSTRING(ROUTINE_NAME,1,4)='spP_' or SUBSTRING(ROUTINE_NAME,1,4)='spD_' ) " +
                    " and (ROUTINE_NAME Like '%"+cadena+"%')"+
                    "order by ROUTINE_NAME";
                //Console.WriteLine(sqltext);
                using (SqlCommand cmd = new(sqltext, connection))
                {
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StProcs s = new StProcs
                            {
                                 
                                id = reader["ID"].ToString(),
                                nombre = reader["nombre"].ToString(),
                                definicion = reader["definicion"].ToString()
                               
                            };
                            stprocs.Add(s);

                        }
                    }
                }
                connection.Close();
            }
            return stprocs;

        }
    }
}

/*
 * app.get('/st_procs', (req, res) 
 * 
 `Select ROUTINE_NAME,ROUTINE_DEFINITION from INFORMATION_SCHEMA.routines
             where ROUTINE_TYPE = 'PROCEDURE' and 
             ( SUBSTRING(ROUTINE_NAME,1,4)='spG_' or SUBSTRING(ROUTINE_NAME,1,4)='spP_' )
             order by ROUTINE_NAME`
 */
