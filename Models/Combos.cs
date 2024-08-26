namespace ApiRestRs.Models
{
    public class ComboSec
    {
        public int idSeccion { get; set; }
        public int idPlato { get; set; }
        public string? descripcion { get; set; }
        public decimal cantMax { get; set; }
        public int orden { get; set; }
        public bool autocompletar { get; set; }
        public bool imprimirEnAceptacion { get; set; }
        public bool seleccionarUno { get; set; }
        public int idTamanio { get; set; }
        public string? descCorta { get; set; }
        public string? tamanio { get; set; }
        public int platoSel { get; set; }
        public string? idTipoConsumo { get; set; }
    }

    public class ComboDet
    {
        public int idSeccion { get; set; }
        public int idPlato { get; set; }
        public string? descripcion { get; set; }
        public int idTamanio { get; set; }
        public string? descCorta { get; set; }
        public string? idTipoConsumo { get; set; }
        public int cantGustos { get; set; }
        public string? tamanio { get; set; }
        public int idSectorExped { get; set; }
        public int impCentralizada { get; set; }

    }
}
