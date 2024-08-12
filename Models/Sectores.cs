namespace ApiRestRs.Models
{
    public class Sectores
    {
        public int IdSector { get; set; }
        public string? Descripcion { get; set; }
        public int Comensales { get; set; }
        
    }
    public class LugSectImpre
    {
        public int idLugarExped { get; set; }
        public int idSectorExped { get; set; }
        public int idImpresora { get; set; }
        public string? descripcion { get; set; }

    }

    public class MensXcomanda
    {
        public string descripcion { get; set; }
        public int idMozo { get; set; }        
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public int nroMesa { get; set; }
        public int idSectorExped { get; set; }
        public int idImpresora { get; set; }


    }
}
