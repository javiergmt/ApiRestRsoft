using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ApiRestRs.Controllers
{
    //[Route("[controller]")]
    [ApiController]
    public class MesaOperarController
    {
        public readonly string? con;
        public MesaOperarController(IConfiguration configuration)
        {
            con = configuration.GetConnectionString("conexion") + " Password=6736";
        }

        [HttpPost]
        [Route("mesa_bloquear")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] MesaOperar m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaBloquear", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", m.NroMesa);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new JsonResult(new { res = "OK" , mesa = m.NroMesa});
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }

                }
            }
       
        }
       
        [HttpPost]
        [Route("mesa_desbloquear")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] MesaDesbloquear m)
        {
            //var result = new Json({"resultado": "bloq"});

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaDesBloquear", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", m.NroMesa);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new JsonResult(new { res = "OK", mesa = m.NroMesa });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }
                   

                }
            }
             
        }

        [HttpPost]
        [Route("mesa_abrir")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] MesaAbrir m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaAbrir", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", m.nromesa);
                    cmd.Parameters.AddWithValue("@Mozo", m.mozo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new JsonResult(new { res = "OK", mesa = m.nromesa, mozo = m.mozo });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }

        [HttpPost]
        [Route("mesa_det")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] EnMesaDet m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaDet", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", m.nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", m.idDetalle);
                    cmd.Parameters.AddWithValue("@idPlato", m.idPlato);
                    cmd.Parameters.AddWithValue("@cant", m.cant);
                    cmd.Parameters.AddWithValue("@pcioUnit", m.pcioUnit);
                    cmd.Parameters.AddWithValue("@importe", m.importe);
                    cmd.Parameters.AddWithValue("@obs", m.obs);
                    cmd.Parameters.AddWithValue("@idTamanio", m.idTamanio);
                    cmd.Parameters.AddWithValue("@Tamanio", m.tamanio);
                    cmd.Parameters.AddWithValue("@procesado", m.procesado);
                    cmd.Parameters.AddWithValue("@hora", m.hora);
                    cmd.Parameters.AddWithValue("@idMozo", m.idMozo);
                    cmd.Parameters.AddWithValue("@idUsuario", m.@idUsuario);
                    cmd.Parameters.AddWithValue("@cocinado", m.cocinado);
                    cmd.Parameters.AddWithValue("@esEntrada", m.esEntrada);
                    cmd.Parameters.AddWithValue("@descripcion", m.descripcion);
                    cmd.Parameters.AddWithValue("@fechaHora", m.fechaHora);
                    cmd.Parameters.AddWithValue("@comanda", m.comanda);
                  
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, idDetalle = m.idDetalle });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }

        [HttpPost]
        [Route("mesa_det_gustos")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] EnMesaDetGustos m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaDetGustos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", m.nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", m.idDetalle);
                    cmd.Parameters.AddWithValue("@idGusto", m.idGusto);
                    cmd.Parameters.AddWithValue("@descripcion", m.descripcion);
                    
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, idDetalle = m.idDetalle });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }


        [HttpPost]
        [Route("mesa_det_combos")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] EnMesaDetCombos m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaDetCombos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", m.nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", m.idDetalle);
                    cmd.Parameters.AddWithValue("@idSeccion", m.idSeccion);
                    cmd.Parameters.AddWithValue("@idPlato", m.idPlato);
                    cmd.Parameters.AddWithValue("@cant", m.cant);
                    cmd.Parameters.AddWithValue("@procesado", m.procesado);
                    cmd.Parameters.AddWithValue("@idTamanio", m.idTamanio);
                    cmd.Parameters.AddWithValue("@obs", m.obs);
                    cmd.Parameters.AddWithValue("@cocinado", m.cocinado);
                    cmd.Parameters.AddWithValue("@fechaHora", m.fechaHora);
                    cmd.Parameters.AddWithValue("@comanda", m.comanda);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, idDetalle = m.idDetalle });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }

        [HttpPost]
        [Route("mesa_det_combos_gustos")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] EnMesaDetCombosGustos m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaDetCombosGustos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", m.nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", m.idDetalle);
                    cmd.Parameters.AddWithValue("@idSeccion", m.idSeccion);
                    cmd.Parameters.AddWithValue("@idGusto", m.idPlato);
                    cmd.Parameters.AddWithValue("@idGusto", m.idGusto);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, idDetalle = m.idDetalle });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }

        [HttpPost]
        [Route("mesa_comensales")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] MesaComensales m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaComensales", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", m.nroMesa);
                    cmd.Parameters.AddWithValue("@cant", m.cant);
                    
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }

        [HttpDelete]
        [Route("mesa_borrar")]
        [EnableCors("MyCors")]
        public ActionResult Delete([FromBody] MesaBorrar m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spD_Mesa", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", m.nroMesa);
                   
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }

    }
}
