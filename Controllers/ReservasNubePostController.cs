using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using ApiRestRs.Authentication;

namespace ApiRestRs.Controllers
{
    [ApiController]
    public class ReservasNubePostController
    {
        public string? con;
        public ReservasNubePostController(IConfiguration configuration)
        {
            string HeaderBD = configuration.GetConnectionString("reservas");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
        }

        [HttpPost]
        [Route("reserva_nube_grabar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] Reservas_Nube r)
        {
            
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.Parameters.Clear();
                if (r.idReserva == 0)
                    cmd.CommandText = "Insert Into Reservas(idResto ,fecha, idTurno, nombre, hora, cant, " +
                    "obs, telefono,email,aceptada,descargada) " +
                    " VALUES( @idResto, @fecha, @idTurno, @nombre, @hora, @cant," +
                    " @obs, @telefono, @email, @aceptada, @descargada ) ";
                else
                    cmd.CommandText = "Update Reservas set idResto = @idResto, fecha = @fecha, idTurno = @idTurno, nombre = @nombre, hora = @hora, cant = @cant, " +
                    "obs = @obs, telefono = @telefono, email = @email, aceptada = @aceptada, descargada = @descargada " +
                    " where idReserva = @idReserva";

                
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idReserva", Value = r.idReserva });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = r.idResto });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@fecha", Value = r.fecha });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idTurno", Value = r.idTurno });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@nombre", Value = r.nombre });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@hora", Value = r.hora });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@cant", Value = r.cant });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@obs", Value = r.obs });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@telefono", Value = r.telefono });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@email", Value = r.email });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@aceptada", Value = r.aceptada });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@descargada", Value = r.descargada });


                cmd.ExecuteNonQuery();

                connection.Close();
                return new JsonResult(new { res = 0, mensaje = "ok" });

            }
        }

        [HttpPost]
        [Route("bloqueos_nube_grabar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] Bloqueos_Nube r)
        {

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.Parameters.Clear();
                if (r.idBloqueo == 0) 
                    cmd.CommandText = "Insert Into Bloqueos(idResto ,fecha, idTurno, obs) " +
                    " VALUES( @idResto, @fecha, @idTurno, @obs )";
                else
                    cmd.CommandText = "Update Bloqueos set idResto = @idResto, fecha = @fecha, idTurno = @idTurno, obs = @obs "+
                        " where idBloqueo = @idBloqueo";
               

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idBloqueo", Value = r.idBloqueo });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = r.idResto });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@fecha", Value = r.fecha });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idTurno", Value = r.idTurno });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@obs", Value = r.obs });

                cmd.ExecuteNonQuery();

                connection.Close();
                return new JsonResult(new { res = 0, mensaje = "ok" });

            }
        }

        [HttpPost]
        [Route("shows_nube_grabar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] Shows_Nube r)
        {

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.Parameters.Clear();
                if (r.idShow == 0)
                    cmd.CommandText = "Insert Into Shows(idResto ,fecha, titulo, descripcion,imagen,hsAnticipacion, idTurno) " +
                    " VALUES( @idResto, @fecha, @titulo, @descripcion, @imagen, @hsAnticipacion, @idTurno )";
                else
                    cmd.CommandText = "Update Shows set idResto = @idResto, fecha = @fecha, titulo = @titulo, descripcion = @descripcion " +
                        ", imagen = @imagen, hsAnticipacion = @hsAnticipacion, idTurno = @idTurno where idShow = @idShow";
              
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idShow", Value = r.idShow });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = r.idResto });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@fecha", Value = r.fecha });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@titulo", Value = r.titulo });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@descripcion", Value = r.descripcion });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@imagen", Value = r.imagen });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@hsAnticipacion", Value = r.hsAnticipacion });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idTurno", Value = r.idTurno });
                cmd.ExecuteNonQuery();

                connection.Close();
                return new JsonResult(new { res = 0, mensaje = "ok" });

            }
        }

        [HttpPost]
        [Route("turnos_nube_grabar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] Turnos_Nube r)
        {

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.Parameters.Clear();
                cmd.CommandText = "Insert Into Turnos(idResto, idTurno ,descripcion, horaDesde, horaHasta, intervaloMin) " +
                    " VALUES( @idResto,@idTurno, @descripcion, @horaDesde, @horaHasta, @intervaloMin )";
              
                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idTurno", Value = Math.Abs(r.idTurno) });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = r.idResto });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@descripcion", Value = r.descripcion });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@horaDesde", Value = r.horaDesde });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@horaHasta", Value = r.horaHasta });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@intervaloMin", Value = r.intervaloMin });

                cmd.ExecuteNonQuery();

                connection.Close();
                return new JsonResult(new { res = 0, mensaje = "ok" });

            }
        }
        [HttpPut]
        [Route("turnos_nube_borrar")]
        [EnableCors("MyCors")]
        public ActionResult Put(IConfiguration configuration, Turnos_Nube_Delete r)
        {

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.Parameters.Clear();
                cmd.CommandText = "Delete Turnos Where idResto = @idResto ";
                   
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idResto", Value = r.idResto });
                
                cmd.ExecuteNonQuery();

                connection.Close();
                return new JsonResult(new { res = 0, mensaje = "ok" });

            }
        }

    }
}
