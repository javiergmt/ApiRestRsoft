namespace ApiRestRs.Models
{
    public class MesaOperar
    {
        public int NroMesa { get; set; }

    }
    public class MesaDesbloquear
    {
        public int NroMesa { get; set; }

    }
    public class MesaAbrir
    {
        public int nromesa { get; set; }
        public int mozo { get; set; }
        
    }
    public class MesaComensales
    {
        public int nroMesa { get; set; }
        public int cant { get; set; }

    }
    public class EnMesaDet
    {
        public int nroMesa { get; set; }
        public int idDetalle { get; set; }
        public int idPlato { get; set; }
        public decimal cant { get; set; }
        public decimal pcioUnit { get; set; }
        public decimal importe { get; set; }
        public string? obs { get; set; }
        public int idTamanio { get; set; }
        public string? tamanio { get; set; }
        public bool procesado { get; set; }
        public string? hora { get; set; }
        public int idMozo { get; set; }
        public int idUsuario { get; set; }
        public bool cocinado { get; set; }
        public bool esEntrada { get; set; }
        public string? descripcion { get; set; }
        public DateTime fechaHora { get; set; }
        public bool comanda { get; set; }
    }


    public class EnMesaDetGustos
    {
        public int nroMesa { get; set; }
        public int idDetalle { get; set; }
        public int idGusto { get; set; }
        public string? descripcion { get; set; }
    }
    

    public class MesaBorrar
    {
        public int nroMesa { get; set; }
        
    }

    public class MesaCerrar
    {
        public int nroMesa { get; set; }
        public decimal pagos { get; set; }
        public int descTipo { get; set; }
        public decimal descImporte { get; set; }
        public int idCliente { get; set; }
        public string? nombre { get; set; }
        public string? direccion { get; set; }
        public string? localidad { get; set; }
        public string? telefono { get; set; }
        public string? telefono2 { get; set; }
        public string? telefono3 { get; set; }
        public string? email { get; set; }
        public int idZona { get; set; }
        public DateTime fechaNac { get; set; }
        public string? cuit { get; set; }
        public string? idIva { get; set; }
        public string? tarjeta { get; set; }
    }
    public class MesaRenglonBorrar
    {
        public int nroMesa { get; set; }
        public int idDetalle { get; set; }
        public int idPlato { get; set; }
        public string? idTipoConsumo { get; set; }
        public decimal cant { get; set; }
        public string? descripcion { get; set; }
        public decimal pcioUnit { get; set; }
        public string? obs { get; set; }
        public int idTamanio { get; set; }
        public string? tamanio { get; set; }
        public DateTime fecha { get; set; }
        public string? hora { get; set; }
        public int idMozo { get; set; }
        public int idUsuario { get; set; }
        public DateTime fechaHoraElim { get; set; }
        public int idUsuarioElim { get; set; }
        public int idMozoElim { get; set; }
        public int idObs { get; set; }
        public string? observacion { get; set; }
        public string? comentario { get; set; }
        public int puntoDeVenta { get; set; }

    }
    public class EnMesaDetCombos
    {
        public int nroMesa { get; set; }
        public int idDetalle { get; set; }
        public int idSeccion { get; set; }
        public int idPlato { get; set; }
        public decimal cant { get; set; }
        public bool procesado { get; set; }
        public int idTamanio { get; set; }
        public string? obs { get; set; }
        public bool cocinado { get; set; }
        public DateTime fechaHora { get; set; }
        public bool comanda { get; set; }
    }
    public class EnMesaDetCombosGustos
    {
        public int nroMesa { get; set; }
        public int idDetalle { get; set; }
        public int idSeccion { get; set; }
        public int idPlato { get; set; }
        public int idGusto { get; set; }
    }
    public class EnMesaDetCombosGustosM
    {
        public int idSeccion { get; set; }
        public int idPlato { get; set; }
        public int idGusto { get; set; }
        public string? descripcion { get; set; }
    }

    public class EnMesaDetCombosM
    {
        public int idSeccion { get; set; }
        public int idPlato { get; set; }
        public string? descripcion { get; set; }
        public decimal cant { get; set; }
        public bool procesado { get; set; }
        public int idTamanio { get; set; }
        public string? tamanio { get; set; }
        public string? obs { get; set; }
        public bool cocinado { get; set; }
        public DateTime fechaHora { get; set; }
        public bool comanda { get; set; }
        public int idSectorExped { get; set; }
        public int impCentralizada { get; set; }
        public List<EnMesaDetCombosGustosM>? CombosGustos { get; set; }
    }
   
    public class EnMesaDetGustosM
    {
        public int idGusto { get; set; }
        public string? descripcion { get; set; }
    }

    public class EnMesaDetM
    {
        public int nroMesa { get; set; }
        public int idDetalle { get; set; }        
        public int idPlato { get; set; }
        public string? idTipoConsumo { get; set; }
        public decimal cant { get; set; }
        public decimal pcioUnit { get; set; }
        public decimal importe { get; set; }
        public string? obs { get; set; }
        public int idTamanio { get; set; }
        public string? tamanio { get; set; }
        public bool procesado { get; set; }
        public string? hora { get; set; }
        public int idMozo { get; set; }
        public string? nombreMozo { get; set; }
        public int idUsuario { get; set; }
        public bool cocinado { get; set; }
        public bool esEntrada { get; set; }
        public string? descripcion { get; set; }
        public DateTime fechaHora { get; set; }
        public bool comanda { get; set; }
        public int idSectorExped { get; set; }
        public int impCentralizada { get; set; }
        public List<EnMesaDetGustosM>? Gustos { get; set; }
        public List<EnMesaDetCombosM>? Combos { get; set; }
    }

    public class EnMesaDetMult
    {
        public List<EnMesaDetM>? MesaDetM { get; set; }

    }

    public class EnMesaDetRenglon
    {
        public int nroMesa { get; set; }
        public int idDetalle { get; set; }
        public decimal cant { get; set; }
    }
}

