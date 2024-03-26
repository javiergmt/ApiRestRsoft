using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;


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
                                Orden = Convert.ToInt32(reader["Orden"])
                                
                            };
                            subrubros.Add(r);

                        }
                    }
                }

            }
            return subrubros;

        }


    }
}
