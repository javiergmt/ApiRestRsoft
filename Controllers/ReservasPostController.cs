using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using ApiRestRs.Authentication;

namespace ApiRestRs.Controllers
{
    
    [ApiController]
    public class ReservasPostController:ControllerBase

    {
        public string? con;
        public ReservasPostController(IConfiguration configuration)
        {
            string HeaderBD = configuration.GetConnectionString("default");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
        }

        [HttpPost]
        [Route("reserva_cambiar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] ReservaCambiar r)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_ReservaCambiar", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReserva", r.idReserva);
                    cmd.Parameters.AddWithValue("@fecha", r.fecha);
                    cmd.Parameters.AddWithValue("@turno", r.turno);
                    cmd.Parameters.AddWithValue("@nombre", r.nombre);
                    cmd.Parameters.AddWithValue("@hora", r.hora);
                    cmd.Parameters.AddWithValue("@cant", r.cant);
                    cmd.Parameters.AddWithValue("@obs", r.obs);
                    cmd.Parameters.AddWithValue("@telefono", r.telefono);
                    cmd.Parameters.AddWithValue("@idSector", r.idSector);
                    cmd.Parameters.AddWithValue("@mesa", r.mesa);
                    cmd.Parameters.AddWithValue("@idCliente", r.idCliente);
                    cmd.Parameters.AddWithValue("@confirmada", r.confirmada);
                    cmd.Parameters.AddWithValue("@cumplida", r.cumplida);
                    cmd.Parameters.AddWithValue("@usuario", r.usuario);
                    cmd.Parameters.AddWithValue("@email", r.email);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        return new JsonResult(new { res = "OK" });
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return new JsonResult(new { res = ex });
                    }
                }
            }
        }

        [HttpDelete]
        [Route("reserva_borrar")]
        [EnableCors("MyCors")]
        public ActionResult Delete(IConfiguration configuration, [FromBody] ReservaBorrar r)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spD_Reserva", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReserva", r.idReserva);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK" });
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }

        [HttpPost]
        [Route("reserva_conf_cump")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] ReservaConfCump r)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_ReservaConfCump", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReserva", r.idReserva);
                    cmd.Parameters.AddWithValue("@confirmada", r.confirmada);
                    cmd.Parameters.AddWithValue("@cumplida", r.cumplida);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK" });
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }
    }

}
