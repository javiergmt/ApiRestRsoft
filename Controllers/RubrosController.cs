//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
//using static System.Net.Mime.MediaTypeNames;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class RubrosController : ControllerBase
    {
        public readonly string? con;
        public RubrosController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("conexion") + " Password=6736";
        }

        [HttpGet("{sucursal}/{favoritos}/{delivery}")]
        [ActionName("rubros")]
        [EnableCors("MyCors")]
        public IEnumerable<Rubros> Rubros(int sucursal, int favoritos, int delivery) {
            List<Rubros> rubros = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd=new("spG_Rubros", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@favoritos", favoritos);
                    cmd.Parameters.AddWithValue("@delivery", delivery);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Rubros r = new Rubros
                            {
                                IdRubro = Convert.ToInt32(reader["idRubro"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Orden = Convert.ToInt32(reader["Orden"]),
                                Visualizacion = Convert.ToChar(reader["Visualizacion"]),
                                iconoApp = reader["iconoApp"].ToString()
                            };
                            rubros.Add(r);

                        }
                    }
                }
                connection.Close();

            }
            return rubros;

        }

        [HttpGet("{idrubro}")]
        [ActionName("subrubros")]
        [EnableCors("MyCors")]
        public IEnumerable<Subrubros> Subrubros(int idrubro)
        {
            List<Subrubros> subrubros = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_SubRubros", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idRubro", idrubro);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Subrubros r = new Subrubros
                            {
                                IdSubRubro = Convert.ToInt32(reader["idSubRubro"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Imagen = (reader["Imagen"]).ToString(),
                                Orden = reader["Order"] as int? ?? 0

                            };
                            subrubros.Add(r);

                        }
                    }
                }
                connection.Close();

            }
            return subrubros;

        }

        [HttpGet("{sucursal}/{favoritos}/{delivery}")]
        [ActionName("rubros_sub")]
        [EnableCors("MyCors")]
        public IEnumerable<RubrosSub> RubrosSub(int sucursal, int favoritos, int delivery)
        {
            List<RubrosSub> rubros_sub = new();
            
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Rubros", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sucursal", sucursal);
                    cmd.Parameters.AddWithValue("@favoritos", favoritos);
                    cmd.Parameters.AddWithValue("@delivery", delivery);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RubrosSub r = new RubrosSub
                            {
                                IdRubro = Convert.ToInt32(reader["idRubro"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                Orden = Convert.ToInt32(reader["Orden"]),
                                Visualizacion = Convert.ToChar(reader["Visualizacion"]),
                                iconoApp = reader["iconoApp"].ToString()
                                                    
                            };

                            int nRubro = Convert.ToInt32(reader["idRubro"]);
                            
                            List<Subrubros>? subr = new();
                               
                            SqlDataReader dataReader;
                            string sql = "Select * from Sub_Rubros Where idRubro = "+nRubro.ToString();
                            SqlConnection connection2 = new(con);
                            connection2.Open();
                            SqlCommand cmd2 = new SqlCommand(sql, connection2);
                            dataReader = cmd2.ExecuteReader();
                                
                            while (dataReader.Read())
                                {
                                    Subrubros sr = new Subrubros
                                    {
                                    IdSubRubro = Convert.ToInt32(dataReader["idSubrubro"]),
                                    Descripcion = dataReader["Descripcion"].ToString(),
                                    Imagen = "",
                                    Orden = reader["Orden"] as int? ?? 0
                                    };
                                    subr.Add(sr);
                                    
                                }
                            connection2.Close();
                            r.Subrubros = subr;

                            rubros_sub.Add(r);

                        }
                    }
                }
                connection.Close();

            }
            return rubros_sub;

        }

    }
}
