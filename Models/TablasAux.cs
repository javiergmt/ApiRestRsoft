using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiRestRs.Models
{
    public class Clientes
    {
        public int IdCliente { get; set; }
        public string? nombre { get; set; }
        public string? direccion { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public string? telefono2 { get; set; }
        public string? telefono3 { get; set; }
        public string? idCondIva { get; set; }
        public string? cuit { get; set; }
        public string? localidad { get; set; }
        public string? codigoPostal { get; set; }
        public int? idZona { get; set; }
        public DateTime? fechaNac { get; set; }
        public string? obs { get; set; }
        public decimal? credito { get; set; }
        public Boolean? bloquearCredito { get; set; }
        public decimal? porcDesc { get; set; }
        public Boolean? aCtaCte { get; set; }
        public string? idTarjeta { get; set; }
        public bool? activo { get; set; }
        public string? nombreFantasia { get; set; }
        public decimal? perIbMinimo { get; set; }
        public decimal? perIbAlicuota { get; set; }
        public string? perIbTipo { get; set; }
        public Boolean? autFactA { get; set; }
        public DateTime? vtoform8001 { get; set; }

    }

    public class ClientesPedido
    {
        public int IdCliente { get; set; }
        public string? nombre { get; set; }
        public string? direccion { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public string? telefono2 { get; set; }
        public string? telefono3 { get; set; }
        public string? idCondIva { get; set; }
        public string? cuit { get; set; }
        public string? localidad { get; set; }
        public int? idZona { get; set; }
        public DateTime? fechaNac { get; set; }
        public string? idTarjeta { get; set; }

    }
    public class FormasDePago
    {
        public int idForma { get; set; }
        public int id { get; set; }
        public string? forma { get; set; }
        public int? orden { get; set; }

    }

    public class CondicionIva
    {
        public string? idCondicionIva { get; set; }
        public string? descripcion { get; set; }
        public string? letraFact { get; set; }
    }

    public class ZonasClientes
    {
        public int idZona { get; set; }
        public string? descripcion { get; set; }

    }

    public class Repartidores
    {
        public int idRepartidor { get; set; }
        public string? nombre { get; set; }

    }
    public class ObsDescuentos
    {
        public int idObs { get; set; }
        public string? descripcion { get; set; }

    }

    public class Turnos
    {
        public int idTurno { get; set; }
        public string? descripcion { get; set; }
        public string? horaDesde { get; set; }
        public string? horaHasta { get; set; }
    }

    public class ParamDelivery 
    {
       public decimal MaxPorcDesc { get; set; }
    public decimal EnvioDelivery { get; set; }
    public bool DeliveryMostradorFacturar { get; set; }
    public bool DeliveryClientesFacturar { get; set; }
    public bool DeliveryImpAlGuardar { get; set; }
    public bool DeliverySoloPlatosDelivery { get; set; }
    public bool MesasNoPlatosDelivery { get; set; }
    public int DeliveryDemorado { get; set; }
    public int DeliveryMuyDemorado { get; set; }
    public string? DeliveryColorDemorado { get; set; }
    public string? DeliveryColorMuyDemorado { get; set; }
    public decimal PorcDescPagoEfectivo { get; set; }
    public int idMotivoDescPagoEfectivo { get; set; }
    public bool AgruparPlatosIguales { get; set; }
    public bool MostrarResumenDelivery { get; set; }

  }
  public class MensajesComanda
  {
        public int idMensaje { get; set; }
        public string? descripcion { get; set; }
        public int orden { get; set; }
        public int nroMesa { get; set; }
  }

    public class UsuarioPass
    {
        public string? clave { get; set; }
        
    }

    public class PuntoSectores
    {
        public int sucursal { get; set; }
        public int idSector { get; set; }

    }

    public class PuntoNoRubros
    {
        public int sucursal { get; set; }
        public int idRubro { get; set; }

    }
}
