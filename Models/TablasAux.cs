﻿namespace ApiRestRs.Models
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
}