using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using ApiRestRs.Authentication;
using System.Drawing;

namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class ReservasNubeController
    {
        public string? con;
        public ReservasNubeController(IConfiguration configuration)
        {
            string HeaderBD = configuration.GetConnectionString("reservas");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
        }

        [HttpGet("{idResto}/{idOper}")]
        [ActionName("reservas_nube")]
        [EnableCors("MyCors")]
        public IEnumerable<Reservas_Nube> Reservas(IConfiguration configuration, int idResto, int idOper)
        {
          
            List<Reservas_Nube> reservas = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();

                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {

                    cmd.Parameters.Clear();
                    if (idOper == 0)
                    {
                        cmd.CommandText = "Select * From Reservas Where idResto = @idResto";
                    }
                    else
                    {
                        cmd.CommandText = "Select * From Reservas Where idResto = @idResto and aceptada = 1 and descargada = 0";
                       
                    }
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = idResto });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reservas_Nube r = new Reservas_Nube
                            {

                                idReserva = Convert.ToInt32(reader["idReserva"]),
                                idResto = Convert.ToInt32(reader["idResto"]),
                                fecha = Convert.ToDateTime(reader["fecha"]),
                                idTurno = Convert.ToInt32(reader["idTurno"]),
                                nombre = reader["nombre"].ToString(),
                                hora = reader["hora"].ToString(),
                                cant = Convert.ToInt32(reader["cant"]),
                                obs = reader["obs"].ToString(),
                                telefono = reader["telefono"].ToString(),
                                email = reader["email"].ToString(),
                                aceptada = Convert.ToInt32(reader["aceptada"]),
                                descargada = Convert.ToBoolean(reader["descargada"]),


                            };
                            reservas.Add(r);
                            // Estas Reservas hay que insertarlas en la base de datos local
                        }

                    }
                    if (idOper == 1 && reservas.Count > 0 )
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "Update Reservas Set descargada = 1 Where idResto = @idResto and aceptada = 1 and descargada = 0";
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = idResto });

                        cmd.ExecuteNonQuery();
                    }
                   
                   
                    transaction.Commit();
                    connection.Close();
                    return reservas;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    connection.Close();
                    //return new JsonResult(new { res = ex });
                    return null;
                }
            }
           
        }


        [HttpGet("{idResto}")]
        [ActionName("bloqueos_nube")]
        [EnableCors("MyCors")]
        public IEnumerable<Bloqueos_Nube> Bloqueos(IConfiguration configuration, int idResto)
        {

            List<Bloqueos_Nube> bloqueos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();

                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {

                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select * From Bloqueos Where idResto = @idResto";
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = idResto });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bloqueos_Nube r = new Bloqueos_Nube
                            {
                                idResto = Convert.ToInt32(reader["idResto"]),
                                idBloqueo = Convert.ToInt32(reader["idBloqueo"]),
                                fecha = Convert.ToDateTime(reader["fecha"]),
                                idTurno = Convert.ToInt32(reader["idTurno"]),                           
                                obs = reader["obs"].ToString(),
                           

                            };
                            bloqueos.Add(r);
                            // Estas Reservas hay que insertarlas en la base de datos local
                        }

                    }

                    transaction.Commit();
                    connection.Close();
                    return bloqueos;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    connection.Close();
                    //return new JsonResult(new { res = ex });
                    return null;
                }
            }

        }

        [HttpGet("{idResto}")]
        [ActionName("turnos_nube")]
        [EnableCors("MyCors")]
        public IEnumerable<Turnos_Nube> Turnos(IConfiguration configuration, int idResto)
        {

            List<Turnos_Nube>  turnos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();

                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {

                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select * From Turnos Where idResto = @idResto";
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = idResto });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Turnos_Nube r = new Turnos_Nube
                            {
                                idResto = Convert.ToInt32(reader["idResto"]),
                                idTurno = Convert.ToInt32(reader["idTurno"]),
                                descripcion = reader["descripcion"].ToString(),
                                horaDesde = reader["horaDesde"].ToString(),
                                horaHasta = reader["horaHasta"].ToString(),
                                intervaloMin = Convert.ToInt32(reader["intervaloMin"]),


                            };
                            turnos.Add(r);
                           
                        }

                    }
                    transaction.Commit();
                    connection.Close();
                    return turnos;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    connection.Close();
                    //return new JsonResult(new { res = ex });
                    return null;
                }
            }

        }

        [HttpGet("{idResto}/{idShow}")]
        [ActionName("shows_nube")]
        [EnableCors("MyCors")]
        public IEnumerable<Shows_Nube> Shows(IConfiguration configuration, int idResto, int idShow)
        {

            List<Shows_Nube> shows = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();

                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {

                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select * From Shows Where (@idResto = idResto) and (@idShow = 0 or @idShow = idShow) ";
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = idResto });
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idShow", Value = idShow });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Shows_Nube r = new Shows_Nube
                            {
                                idResto = Convert.ToInt32(reader["idResto"]),
                                idShow = Convert.ToInt32(reader["idShow"]),
                                fecha = Convert.ToDateTime(reader["fecha"]),
                                titulo = reader["titulo"].ToString(),
                                descripcion = reader["descripcion"].ToString(),
                                imagen = reader["imagen"].ToString(),
                                hsAnticipacion = Convert.ToInt32(reader["hsAnticipacion"]),
                                idTurno = Convert.ToInt32(reader["idTurno"]),

                            };
                            shows.Add(r);

                        }

                    }
                    transaction.Commit();
                    connection.Close();
                    return shows;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    connection.Close();
                    //return new JsonResult(new { res = ex });
                    return null;
                }
            }

        }
    

        [HttpGet("{idResto}")]
        [ActionName("resto_nube")]
        [EnableCors("MyCors")]
        public IEnumerable<Resto_Nube> Resto_Nube(IConfiguration configuration, int idResto)
        {

            List<Resto_Nube> resto = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();

                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {

                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select * From Resto Where ( @idResto=0) OR (@idResto = idResto)";
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = idResto });


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Resto_Nube r = new Resto_Nube
                            {

                                idResto = Convert.ToInt32(reader["idResto"]),
                                descripcion = reader["descripcion"].ToString(),
                                imagen = reader["imagen"].ToString(),
                                nombreResto = reader["nombreResto"].ToString(),
                                hsAnticipacion = Convert.ToInt32(reader["hsAnticipacion"]),
                                linkCarta = reader["linkCarta"].ToString(),
                                linkGoogle = reader["linkGoogle"].ToString(),
                                linkWeb = reader["linkWeb"].ToString(),
                                clave = reader["clave"].ToString(),

                            };
                            resto.Add(r);

                        }

                    }
                    transaction.Commit();
                    connection.Close();
                    return resto;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    connection.Close();
                    //return new JsonResult(new { res = ex });
                    return null;
                }
            }

        }
    }
}
