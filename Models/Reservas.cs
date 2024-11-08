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

    public class Reservas_Nube
    {
        public int idReserva { get; set; }
        public int idResto { get; set; }
        public DateTime? fecha { get; set; }
        public int idTurno { get; set; }
        public string? nombre { get; set; }
        public string? hora { get; set; }
        public int cant { get; set; }
        public string? obs { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }

        public int aceptada { get; set; }
        public bool descargada { get; set; }


    }

    public class Bloqueos_Nube
    {
        public int idResto { get; set; }
        public int idBloqueo { get; set; }
        public DateTime? fecha { get; set; }
        public int idTurno { get; set; }
        public string? obs { get; set; }
    }
    public class Turnos_Nube
    {
        public int idResto { get; set; }
        public int idTurno { get; set; }
        public string? descripcion { get; set; }
        public string? horaDesde { get; set; }
        public string? horaHasta { get; set; }
        public int intervaloMin { get; set; }
    }

    public class Shows_Nube
    {
        public int idResto { get; set; }
        public int idShow { get; set; }
        public DateTime? fecha { get; set; }
        public string? descripcion { get; set; }
        public string? imagen { get; set; }
        public int hsAnticipacion { get; set; }
    }

    public class Resto_Nube
    {
        public int idResto { get; set; }
        public string? nombreResto { get; set; }
        public string? imagen { get; set; }
        public string? descripcion { get; set; }
        public int hsAnticipacion { get; set; }
        public string? linkCarta { get; set; }
        public string? linkWeb { get; set; }
        public string? linkGoogle { get; set; }
        public string? clave { get; set; }
    }

    public class Turnos_Nube_Delete
    {
        public int idResto { get; set; }

    }
}
    
