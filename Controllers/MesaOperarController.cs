using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Drawing;

namespace ApiRestRs.Controllers

{
    //[Route("[controller]")]
    [ApiController]
    public class MesaOperarController
    {
        public readonly string? con;

        public object PrinterSettings { get; private set; }

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
                        return new JsonResult(new { res = "OK", mesa = m.nromesa, m.mozo });
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
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, m.idDetalle });
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
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, m.idDetalle });
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
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, m.idDetalle });
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
                        return new JsonResult(new { res = "OK", mesa = m.nroMesa, m.idDetalle });
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

        [HttpDelete]
        [Route("mesa_renglon_borrar")]
        [EnableCors("MyCors")]
        public ActionResult Delete([FromBody] MesaRenglonBorrar m)
        {
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


        [HttpPost]
        [Route("mesa_det_mult")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] EnMesaDetMult m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                //command.CommandText =
                //    "Insert into Region (RegionID, RegionDescription) VALUES (100, 'Description')";
                //command.ExecuteNonQuery();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                                       

                if (m.MesaDetM != null)
                {
                    try
                    {

                        foreach (var Mdet in m.MesaDetM)
                        {
                            command.Parameters.Clear();

                            // Grabo detalle
                            command.CommandText =
                            "Insert into En_MesaDet (nroMesa, idDetalle,idPlato,cant,pcioUnit,importe,obs,idTamanio," +
                            "tamanio, procesado, hora, idMozo, idUsuario, cocinado, esEntrada, descripcion," +
                            "fechaHora, comanda) " +
                            " VALUES(@nroMesa, @idDetalle, @idPlato, @cant, @pcioUnit, @importe, @obs, @idTamanio, " +
                            "@tamanio, @procesado, @hora, @idMozo, @idUsuario, @cocinado, @esEntrada, @descripcion," +
                            "@fechaHora, @comanda) ";
                            
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@nroMesa", Value = Mdet.nroMesa });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idDetalle", Value = Mdet.idDetalle });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idPlato", Value = Mdet.idPlato });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@cant", Value = Mdet.cant });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@pcioUnit", Value = Mdet.pcioUnit });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@importe", Value = Mdet.importe });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@obs", Value = Mdet.obs });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idTamanio", Value = Mdet.idTamanio });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@tamanio", Value = Mdet.tamanio });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@procesado", Value = Mdet.procesado });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@hora", Value = Mdet.hora });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idMozo", Value = Mdet.idMozo });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@idUsuario", Value = Mdet.idUsuario });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@cocinado", Value = Mdet.cocinado });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@esEntrada", Value = Mdet.esEntrada });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@descripcion", Value = Mdet.descripcion });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@fechaHora", Value = Mdet.fechaHora });
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@comanda", Value = Mdet.comanda });

                            command.ExecuteNonQuery();

                            if (Mdet.Gustos != null)
                            {
                                foreach (var Mgus in Mdet.Gustos)
                                {
                                    // Grabo gustos
                                    command.CommandText =
                                    "Insert into En_MesaDet_Gustos (NroMesa, IdDetalle,idGusto, Descripcion) " +
                                    "VALUES (" + Mdet.nroMesa + "," + Mdet.idDetalle + "," + Mgus.idGusto +
                                    ", '"+Mgus.descripcion + "')";

                                    command.ExecuteNonQuery();
                                };
                            }

                            if (Mdet.Combos != null)
                            {
                                foreach (var MCom in Mdet.Combos)
                                {
                                    // Grabo combos
                                    command.CommandText =
                                    "Insert into En_MesaDet_Combos (NroMesa, IdDetalle,idSeccion, idPlato," +
                                    "Cant,Procesado,IdTamanio,Obs,Cocinado,FechaHora,Comanda) " +
                                    "VALUES (" + Mdet.nroMesa + "," + Mdet.idDetalle + "," + MCom.idSeccion + "," +
                                     MCom.idPlato + ", " + MCom.cant + ",'" + MCom.procesado + "'," + MCom.idTamanio +
                                     ",'" + MCom.obs + "','" + MCom.cocinado + "','" + MCom.fechaHora + "','" +
                                     MCom.comanda + "')";
                    
                                    command.ExecuteNonQuery();
                                    if (MCom.CombosGustos != null)
                                    {
                                        foreach (var MComGust in MCom.CombosGustos)
                                        {
                                            // Grabo combos gustos
                                            command.CommandText =
                                            "Insert into En_MesaDet_Combos_Gustos (NroMesa, IdDetalle,idSeccion,idPlato,idGusto) " +
                                            "VALUES (" + Mdet.nroMesa + "," + Mdet.idDetalle + "," + MComGust.idSeccion +
                                            "," + MComGust.idPlato + "," + MComGust.idGusto + ")";

                                            command.ExecuteNonQuery();
                                        };
                                    }
                                };
                            }
                           
                        }
                        transaction.Commit();
                        Imprimir.ImprimirComanda( m , con);
                        return new JsonResult(new { res = 0, mensaje = "commit" });
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                        //Console.WriteLine("  Message: {0}", ex.Message);

                        // Attempt to roll back the transaction.
                        try
                        {
                            transaction.Rollback();
                            return new JsonResult(new { res = 1, mensaje = "rollback: "+ex.Message });
                        }
                        catch (Exception ex2)
                        {
                            // This catch block will handle any errors that may have occurred
                            // on the server that would cause the rollback to fail, such as
                            // a closed connection.
                            //Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                            //Console.WriteLine("  Message: {0}", ex2.Message);
                            return new JsonResult(new { res = -1, mensaje = "error: "+ex2.Message });
                        }
                    }
                }
                return new JsonResult(new { res = 0, mensaje = "vacio" });
            }

            
        }

        private void ImprimirComanda(EnMesaDetM enMesaDetM, EnMesaDetMult m)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("grabaMensaje")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] MensXcomanda m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_GrabaMensaje", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", m.descripcion);
                    cmd.Parameters.AddWithValue("@idMozo", m.idMozo);
                    cmd.Parameters.AddWithValue("@idUsuario", m.idUsuario);
                    cmd.Parameters.AddWithValue("@nroMesa", m.nroMesa);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        // Imprimir.ImprimirMensaje(m.idSectorExped, m.idImpresora,m.descripcion,con);
                        return new JsonResult(new { res = "OK" });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }

        [HttpPost]
        [Route("actIconoRubro")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] ActIconoRubro r)
        {
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
                        return new JsonResult(new { res = "OK", rubro = r.IdRubro });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new { res = ex });
                    }


                }
            }
        }


        [HttpPost]
        [Route("mesa_cerrar")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] MesaCerrar m)
        {
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spP_MesaCerrar", connection))
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

        [HttpPost]
        [Route("mesa_renglon_cambiar")]
        [EnableCors("MyCors")]
        public ActionResult Post([FromBody] EnMesaDetRenglon m)
        {
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
