using ApiRestRs.Authentication;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using System.Drawing;

namespace ApiRestRs.Controllers

{
    //[Route("[controller]")]
    [ApiController]
    public class MesaOperarController : ControllerBase
    {
        public string? con;

        public object? PrinterSettings { get; private set; }
        public object Grabar { get; private set; }

        public MesaOperarController(IConfiguration configuration)
        {
            string HeaderBD = configuration.GetConnectionString("default");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
        }

        [HttpPost]
        [Route("mesa_bloquear")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration,[FromBody] MesaOperar m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaBloquear", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroMesa", m.NroMesa);
                    cmd.Parameters.AddWithValue("@idMozo", m.idMozo);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.NroMesa });
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
        [Route("mesa_desbloquear")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] MesaDesbloquear m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

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
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.NroMesa });
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
        [Route("mesa_abrir")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] MesaAbrir m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

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
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nromesa, m.mozo });
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
        [Route("mesa_det")]
        [EnableCors("MyCors")]


        public ActionResult Post(IConfiguration configuration, [FromBody] EnMesaDet m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

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
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, m.idDetalle });
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
        [Route("mesa_det_gustos")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] EnMesaDetGustos m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

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
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, m.idDetalle });
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
        [Route("mesa_det_combos")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] EnMesaDetCombos m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

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
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, m.idDetalle });
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
        [Route("mesa_det_combos_gustos")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] EnMesaDetCombosGustos m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

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
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, m.idDetalle });
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
        [Route("mesa_comensales")]
        [EnableCors("MyCors")]
   
        public ActionResult Post(IConfiguration configuration, [FromBody] MesaComensales m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

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
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa });
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
        [Route("mesa_borrar")]
        [EnableCors("MyCors")]
        public ActionResult Delete(IConfiguration configuration, [FromBody] MesaBorrar m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

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
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa });
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
        [Route("mesa_renglon_borrar")]
        [EnableCors("MyCors")]
        public ActionResult Delete(IConfiguration configuration, [FromBody] MesaRenglonBorrar m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spD_MesaRenglon", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", m.nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", m.idDetalle);
                    cmd.Parameters.AddWithValue("@idPlato", m.idPlato);
                    cmd.Parameters.AddWithValue("@idTipoConsumo", m.idTipoConsumo);
                    cmd.Parameters.AddWithValue("@cant", m.cant);
                    cmd.Parameters.AddWithValue("@descripcion", m.descripcion);
                    cmd.Parameters.AddWithValue("@pcioUnit", m.pcioUnit);
                    cmd.Parameters.AddWithValue("@obs", m.obs);
                    cmd.Parameters.AddWithValue("@idTamanio", m.idTamanio);
                    cmd.Parameters.AddWithValue("@tamanio", m.tamanio);
                    cmd.Parameters.AddWithValue("@fecha", m.fecha);
                    cmd.Parameters.AddWithValue("@hora", m.hora);
                    cmd.Parameters.AddWithValue("@idMozo", m.idMozo);
                    cmd.Parameters.AddWithValue("@idUsuario", m.idUsuario);
                    cmd.Parameters.AddWithValue("@fechaHoraElim", m.fechaHoraElim);
                    cmd.Parameters.AddWithValue("@idUsuarioElim", m.idUsuarioElim);
                    cmd.Parameters.AddWithValue("@idMozoElim", m.idMozoElim);
                    cmd.Parameters.AddWithValue("@idObs", m.idObs);
                    cmd.Parameters.AddWithValue("@observacion", m.observacion);
                    cmd.Parameters.AddWithValue("@comentario", m.comentario);
                    cmd.Parameters.AddWithValue("@puntoDeVenta", m.puntoDeVenta);
                    cmd.Parameters.AddWithValue("@idSeccion", m.idSeccion);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa });
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
        [Route("mesa_det_mult")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] EnMesaDetMult m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            return Imprimir.GrabarMulti(m, con, true);

        }
    

        [HttpPost]
        [Route("actIconoRubro")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] ActIconoRubro r)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_ActIconoRubro", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idRubro", r.IdRubro);
                    cmd.Parameters.AddWithValue("@iconoApp", r.iconoApp);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK", rubro = r.IdRubro });
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
        [Route("mesa_cerrar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] MesaCerrar m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaCerrar", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", m.nroMesa);
                    cmd.Parameters.AddWithValue("@pagos", m.pagos);
                    cmd.Parameters.AddWithValue("@descTipo", m.descTipo);
                    cmd.Parameters.AddWithValue("@descImporte", m.descImporte);
                    cmd.Parameters.AddWithValue("@idCliente", m.idCliente);
                    cmd.Parameters.AddWithValue("@nombre", m.nombre);
                    cmd.Parameters.AddWithValue("@direccion", m.direccion);
                    cmd.Parameters.AddWithValue("@localidad", m.localidad);
                    cmd.Parameters.AddWithValue("@telefono", m.telefono);
                    cmd.Parameters.AddWithValue("@telefono2", m.telefono2);
                    cmd.Parameters.AddWithValue("@telefono3", m.telefono3);
                    cmd.Parameters.AddWithValue("@email", m.email);
                    cmd.Parameters.AddWithValue("@idZona", m.idZona);
                    cmd.Parameters.AddWithValue("@fechaNac", m.fechaNac);
                    cmd.Parameters.AddWithValue("@idIva", m.idIva);
                    cmd.Parameters.AddWithValue("@cuit", m.cuit);
                    cmd.Parameters.AddWithValue("@tarjeta", m.tarjeta);

                    try
                    {
                        //Imprimir.ImprimirAceptacion(m, con);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa });
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
        [Route("mesa_cerrar_mozo")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] MesaCerrarMozo m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaCerrarMozo", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", m.nroMesa);
                    

                    try
                    {
                        Imprimir.ImprimirAceptacion(m, con);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa });
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
        [Route("mesa_renglon_cambiar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] EnMesaDetRenglon m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaRenglonCambiar", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nroMesa", m.nroMesa);
                    cmd.Parameters.AddWithValue("@idDetalle", m.idDetalle);
                    cmd.Parameters.AddWithValue("@cant", m.cant);


                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa });
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
        [Route("objeto_plano_cambiar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] MesasObjetos m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_ObjetoCambiar", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idObjeto", m.idObjeto);
                    cmd.Parameters.AddWithValue("@descripcion", m.descripcion);
                    cmd.Parameters.AddWithValue("Forma", m.forma);
                    cmd.Parameters.AddWithValue("@idSector", m.idSector);
                    cmd.Parameters.AddWithValue("@color", m.color);
                    cmd.Parameters.AddWithValue("@penColor", m.penColor);
                    cmd.Parameters.AddWithValue("@penStyle", m.penStyle);
                    cmd.Parameters.AddWithValue("@brushStyle", m.brushStyle);
                    cmd.Parameters.AddWithValue("@posTop", m.posTop);
                    cmd.Parameters.AddWithValue("@posLeft", m.posLeft);
                    cmd.Parameters.AddWithValue("@width", m.width);
                    cmd.Parameters.AddWithValue("@height", m.height);
                    cmd.Parameters.AddWithValue("@puntasRedondeadas", m.puntasRedondeadas);

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
        [Route("objeto_plano_borrar")]
        [EnableCors("MyCors")]
        public ActionResult Delete(IConfiguration configuration, [FromBody] ObjetoBorrar m)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }

            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spD_Objeto", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idObjeto", m.idObjeto);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK"});
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
        [Route("comandar")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] comanda c)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
           
            if (c.platos != null)
            {
                Imprimir.ImprimirComanda2(c, con);
            }
        
            return new JsonResult(new { res = 0, mensaje = "comandado" });


        }

        [HttpPost]
        [Route("graba_mensaje")]
        [EnableCors("MyCors")]
        public ActionResult Post(IConfiguration configuration, [FromBody] MensXcomanda c)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_GrabaMensaje", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", c.descripcion);
                    cmd.Parameters.AddWithValue("@idMozo", c.idMozo);
                    cmd.Parameters.AddWithValue("@idUsuario", c.idUsuario);
                    cmd.Parameters.AddWithValue("@nroMesa", c.nroMesa);


                    Imprimir.ImprimirMensaje(c.idSectorExped, c.idImpresora, c.descripcion, c.nroMesa, c.nombre, con);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return new JsonResult(new { res = "OK", mesa = c.nroMesa });
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        return new JsonResult(new { res = ex });
                    }


                }
            }



        }

    } // Fin de la clase
} // Fin del namespace
