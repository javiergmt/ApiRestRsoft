﻿namespace ApiRestRs.Models
{
    public class Rubros
    {
        public int IdRubro { get; set; }
        public string? Descripcion { get; set; }
        public int? Orden { get; set; }
        public char? Visualizacion { get; set; }
        public string? iconoApp { get; set; }
        public string? Prefijo { get; set; }
    }
    public class Subrubros
    {
        public int IdSubRubro { get; set; }
        public string? Descripcion { get; set; }
        public string? Imagen { get; set; }
        public int Orden { get; set; }
        
    }

    public class RubrosSub
    {
        public int IdRubro { get; set; }
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
        public char Visualizacion { get; set; }
        public string? iconoApp { get; set; }
        public string? Prefijo { get; set; }
        public List<Subrubros>? Subrubros { get; set; }
    }

    public class ActIconoRubro
    {
        public int IdRubro { get; set; }
        public string? iconoApp { get; set; }
    }
}
