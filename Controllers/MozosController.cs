using ApiRestRs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System;

namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class MozosController
    {
        public readonly string? con;
        public MozosController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("conexion") + " Password=6736";
        }

        [HttpGet("{pass}")]
        //[Route("mozos_pass")]
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
                                Sucursal = Convert.ToInt32(reader["Sucursal"]),
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
                                PedirCubiertos = Convert.ToBoolean(reader["PedirCubiertos"]),
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

            }
            //Imprimir.ImprimirComanda();
            return parammozos;

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
