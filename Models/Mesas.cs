namespace ApiRestRs.Models
{
    public class Mesas
    {
        public int NroMesa { get; set; }
        public int IdSector { get; set; }
        public int PosTop { get; set; }
        public int PosLeft { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public char Ocupada { get; set; }
        public int? Nro2 { get; set; }
        public int? IdMozo { get; set; }
        public int? Forma { get; set; }
        public int? Cerrada { get; set; }
        public int? CantSillas { get; set; }
        public int? CantPersonas { get; set; }
        public string? DescMesa { get; set; }
        public int? Activa { get; set; }
        public int? ConPedEnEspera { get; set; }
        public int? Comiendo { get; set; }
        public int? Pendiente { get; set; }
        public int? PorCobrar { get; set; }
        public int? Reservada { get; set; }
        public int? SoloOcupada { get; set; }
        public int? ConPostre { get; set; }

    }

    public class Mesa
    {
        public char Ocupada { get; set; }
        public int? IdMozo { get; set; }
        public int? Cerrada { get; set; }
        public DateTime? Fecha { get; set; }
        public int? CantPersonas { get; set; }
        public int? Activa { get; set; }
        public int? PorCobrar { get; set; }
        public string? Mozo { get; set; }
        public int? Reservada { get; set; }
        public string? NombreReserva { get; set; }
        public int? IdReserva { get; set; }

    }

    public class MesasMozos
    {
        public int NroMesa { get; set; }
        public int Cerrada { get; set; }
        public int Sector { get; set; }
        public decimal Importe { get; set; }
        public string? Descripcion { get; set; }
    }

    public class MesaEnc
    {
        public int nroMesa { get; set; }
        public int? idMozo { get; set; }
        public DateTime? fecha { get; set; }
        public int? cerrada { get; set; }
        public decimal porcDesc { get; set; }
        public int? cantPersonas { get; set; }
        public decimal descPesos { get; set; }
        public DateTime? fechaHoraImp { get; set; }
        public int? idCliente { get; set; }
        public int? idOcupacion { get; set; }
        public int? idSector { get; set; }
        public string? descMesa { get; set; }
        public string? nombre { get; set; }

    }

    public class MesaDet
    {
        public int NroMesa { get; set; }
        public int? IdPlato { get; set; }
        public int? IdDetalle { get; set; }
        public decimal Cant { get; set; }
        public decimal PcioUnit { get; set; }
        public decimal Importe { get; set; }
        public string? Descripcion { get; set; }
        public char Cocido { get; set; }
        public string? idTipoConsumo { get; set; }
        public string? detalles { get; set; }
    }


    public class MesaPagos
    {
        public decimal Pagos { get; set; }


    }

    public class MesaForma
    {
        public int idForma { get; set; }
        public string? descripcion { get; set; }
    }

    public class MesasObjetos
    {
        public int idObjeto { get; set; }
        public string? descripcion { get; set; }
        public int forma { get; set; }
        public int idSector { get; set; }
        public string? color { get; set; }
        public string? penColor { get; set; }
        public int brushStyle { get; set; }
        public int penStyle { get; set; }
        public int posTop { get; set; }
        public int posLeft { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool puntasRedondeadas { get; set; }

    }

    
   
}