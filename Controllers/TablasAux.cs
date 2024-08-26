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

    }
}
