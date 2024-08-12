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
}
