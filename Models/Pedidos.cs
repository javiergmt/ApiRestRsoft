namespace ApiRestRs.Models
{
    public class Pedidos

    {
       public int ptoDeVta { get; set; }
       public int idPedido { get; set; }
       public int CodClie { get; set; }
       public string? Cliente { get; set; }
       public DateTime FechaEntrega { get; set; }
       public string? HoraEntrega { get; set; }
       public string? Direccion { get; set; }
       public decimal? Total { get; set; }
       public decimal? Total1 { get; set; }
       public decimal? Total2 { get; set; }
       public string? sPagaCon { get; set; }
       public string? Telefono { get; set; }
       public string? Repartidor { get; set; }
       public int Cobrado { get; set; }
       public int xMostrador { get; set; }
       public DateTime Fecha { get; set; }
       public string? Hora { get; set; }
       public string? Usuario { get; set; }
       public decimal Minutos { get; set; }
    }

    public class PedEnc
    {
        public int idPedido { get; set; }
        public DateTime fecha { get; set; }
        public string? hora { get; set; }
        public int idCliente { get; set; }
        public DateTime fechaEntrega { get; set; }
        public string? horaEntrega { get; set; }
        public decimal? subtotal { get; set; }
        public decimal? descuento { get; set; }
        public decimal? total { get; set; }
        public decimal? envio { get; set; }
        public decimal? pago { get; set; }
        public decimal? pagaCon { get; set; }
        public string? obs { get; set; }
        public int? idRepartidor { get; set; }
        public string? nombreClie { get; set; }
        public string? direccionClie { get; set; }
        public bool enUso { get; set; }
        public bool Cobrado { get; set; }
        public bool xMostrador { get; set; }
        public int idUsuario { get; set; }
        public int puntoDeVenta { get; set; }
        public bool delivery { get; set; }
        public int tipoDesc { get; set; }
        public decimal descRec { get; set; }
    }

   public class FacturaCrear
    {
        public int idPedido { get; set; }
        public int nroMesa { get; set; }
        public bool pagoEnMesa { get; set; }
        public bool fiscal { get; set; }
        public int idRepartidor { get; set; }
        public int idObsDesc { get; set; }
        public int idUsuario { get; set; }
        public int idCliente { get; set; }
        public decimal total { get; set; }
        public int tipoDesc { get; set; }
        public decimal impDesc { get; set; }

    }

    public class FacturaPagar
    {
        public string? nro { get; set; }
        public decimal importe { get; set; }
        public int idCliente { get; set; }
        public int idFormaPago { get; set; }
        public int idCupon { get; set; }
        public int idMoneda { get; set; }
        public decimal importeMoneda { get; set; }
        public decimal cotizacion { get; set; }
        public decimal billetes { get; set; }

    }

    public class PedidoRenglonCambiar
    {
        public int idPedido { get; set; }
        public int idDetalle { get; set; }
        public decimal cant{ get; set; }
    }
}
