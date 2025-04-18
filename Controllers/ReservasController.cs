﻿using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using ApiRestRs.Models;
using Microsoft.AspNetCore.Cors;
using ApiRestRs.Authentication;

namespace ApiRestRs.Controllers
{
    [Route("[action]")]
    [ApiController]
  
    public class ReservasController : ControllerBase
    {
        public string? con;
        public ReservasController(IConfiguration configuration)
        {
            string HeaderBD = configuration.GetConnectionString("default");
            con = configuration.GetConnectionString("conexion") + " Database = " + HeaderBD + "; Password=6736";
        }

        [HttpGet("")]
        [ActionName("reservas_turnos")]
        [EnableCors("MyCors")]
        public IEnumerable<TurnosReservas> TurnosReservas(IConfiguration configuration)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<TurnosReservas> turnos = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_ReservasTurnos", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TurnosReservas t = new TurnosReservas
                            {
                                idTurno = Convert.ToInt32(reader["idTurno"]),
                                descripcion = reader["descripcion"].ToString(),
                                horaDesde = reader["horaDesde"].ToString(),
                                horaHasta = reader["horaHasta"].ToString(),
                                intervaloMin = Convert.ToInt32(reader["minOcup"])


                            };
                            turnos.Add(t);

                        }
                    }
                }
                connection.Close();

            }
            return turnos;
        }

        [HttpGet("{fecha}/{fechaHasta}/{idTurno}")]
        [ActionName("reservas")]
        [EnableCors("MyCors")]
        public IEnumerable<Reservas> Reservas(IConfiguration configuration, DateTime fecha, DateTime fechaHasta ,int idTurno)
        {
            string? HeadDb = GetHeader.AnalizarHeaders(Request.Headers);
            if (HeadDb != null)
            {
                con = configuration.GetConnectionString("conexion") + " Database = " + HeadDb + "; Password=6736";
            }
            List<Reservas> reservas = new();
            using (SqlConnection connection = new(con))
            {
                connection.Open();
                using (SqlCommand cmd = new("spG_Reservas", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta);
                    cmd.Parameters.AddWithValue("@idTurno", idTurno);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Reservas r = new Reservas
                            {

                                idReserva = Convert.ToInt32(reader["idReserva"]),
                                fecha = Convert.ToDateTime(reader["fecha"]),
                                turno = Convert.ToInt32(reader["turno"]),
                                descTurno = reader["descTurno"].ToString(),
                                nombre = reader["nombre"].ToString(),
                                hora = reader["hora"].ToString(),
                                cant = Convert.ToInt32(reader["cant"]),
                                obs = reader["obs"].ToString(),
                                telefono = reader["telefono"].ToString(),
                                idSector = Convert.ToInt32(reader["idSector"]),
                                sector = reader["sector"].ToString(),
                                mesa = Convert.ToInt32(reader["mesa"]),
                                idCliente = Convert.ToInt32(reader["idCliente"]),
                                nombreClie = reader["nombreClie"].ToString(),
                                confirmada = Convert.ToBoolean(reader["confirmada"]),
                                cumplida = Convert.ToBoolean(reader["cumplida"]),
                                usuario = reader["usuario"].ToString(),
                                email = reader["email"].ToString()


                            };
                            reservas.Add(r);

                        }
                    }
                }
                connection.Close();

            }
            return reservas;
        }

        

    }
}
