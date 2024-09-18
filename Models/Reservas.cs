namespace ApiRestRs.Models
{
    public class Reservas
    {
        public int idReserva { get; set; }
        public DateTime? fecha { get; set; }
        public int turno { get; set; }
        public string? descTurno { get; set; }
        public string? nombre { get; set; }
        public string? hora { get; set; }
        public int cant { get; set; }
        public string? obs { get; set; }
        public string? telefono { get; set; }
        public int idSector { get; set; }
        public string? sector { get; set; }
        public int mesa { get; set; }
        public int idCliente { get; set; }
        public string? nombreClie { get; set; }
        public bool? confirmada { get; set; }
        public bool? cumplida { get; set; }
        public string? usuario { get; set; }

    }

    public class ReservaCambiar
    {
        public int idReserva { get; set; }
        public DateTime? fecha { get; set; }
        public int turno { get; set; }
        public string? nombre { get; set; }
        public string? hora { get; set; }
        public int cant { get; set; }
        public string? obs { get; set; }
        public string? telefono { get; set; }
        public int idSector { get; set; }
        public int mesa { get; set; }
        public int idCliente { get; set; }
        public bool? confirmada { get; set; }
        public bool? cumplida { get; set; }
        public string? usuario { get; set; }

    }

    public class TurnosReservas
    {
        public int? idTurno { get; set; }
        public string? descripcion { get; set; }
        public string? horaDesde { get; set; }
        public string? horaHasta { get; set; }
    }

    public class ReservaBorrar
    {
        public int idReserva { get; set; }

    }

}
