namespace ApiRestRs.Models
{
    public class Platos
    {
        public int IdPlato { get; set; }
        public string? DescCorta { get; set; }
        public decimal Precio { get; set; }
        public bool TamanioUnico { get; set; }
        public string? idTipoConsumo { get; set; }
        public int? idRubro { get; set; }
        public int? idSubRubro { get; set; }
        public int? cantgustos { get; set; }
        public string? codBarra { get; set; }
        public string? colorFondo { get; set; }
    }
    public class PlatosGustos
    {
        public int IdPlato { get; set; }
        public int IdGusto { get; set; }
        public string? DescGusto { get; set; }
        
    }
    public class PlatosTamanios
    {
        public int IdPlato { get; set; }
        public int IdTamanio { get; set; }
        public string? DescTam { get; set; }

    }

    public class PlatoPrecio
    {
        public int IdPlato { get; set; }
        public decimal Precio { get; set; }
       
    }
    public class PlatoEnMesa
    {
        public int Nro_Mesa { get; set; }
        public string? Hora { get; set; }
        public decimal Cant { get; set; }
        public int CodMozo { get; set; }
        public string? Mozo { get; set; }
        public string? CargadoPor { get; set; }

    }

    public class PlatosObs
    {
        public int idObs { get; set; }
        public string? descripcion { get; set; }

    }

    public class PlatoInfo
    {
        public int idPlato { get; set; }
        public string? descripcion { get; set; }
        public decimal cant { get; set; }
        public string? tamanio { get; set; }
        public string? hora { get; set; }
        public string? mozo { get; set; }
        public string? usuario { get; set; }
        public int gustos { get; set; }
        public string? idTipoConsumo { get; set; }
        public string? obs { get; set; }

    }
    public class PlatoInfoGustos
    {
        public string? descripcion { get; set; }

    }
    public class PlatoInfoCombo
    {
        public int idPlato { get; set; }
        public decimal cant { get; set; }
        public string? descripcion { get; set; }

    }
}
