namespace ApiRestRs.Models
{
    public class Mozos
    {
        public int IdMozo { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
        public string? Password { get; set; }
        public string? Tecla { get; set; }
        public int IdTipoMozo { get; set; }
        public string? NroPager { get; set; }
        public int Orden { get; set; }

    }

    public class ParamMozos
    {
        public string? ColorMesaCerrada { get; set; }
        public string? ColorMesaComiendo { get; set; }
        public string? ColorMesaComiendoEsperando { get; set; }
        public string? ColorMesaEperando { get; set; }
        public string? ColorMesaNormal { get; set; }
        public string? ColorMesaOcupada { get; set; }
        public string? ColorMesaPorCobrar { get; set; }
        public string? ColorMesaSinPed { get; set; }
        public string? ColorPostre { get; set; }
        public int Sucursal { get; set; }
        public bool ActivarSubEstadosMesa { get; set; }
        public bool ModificaPrecioEnMesa { get; set; }
        public bool PermiteDescLibre { get; set; }
        public bool PermitePrecioCero { get; set; }
        public bool AutorizarElimiarPlatoEnMesa { get; set; }
        public bool MarcarMesaSinCobrar { get; set; }
        public bool MozosCierranMesa { get; set; }
        public bool MostrarRubroFavoritos { get; set; }
        public bool OcultarTotalMesaMozos { get; set; }
        public bool PedirMotElimRenglon { get; set; }
        public bool PedirCubiertos { get; set; }
        public int idCubiertos { get; set; }
        public int idTurnoCubierto { get; set; }
        public string? Nombre { get; set; }
        public int idPagWeb { get; set; }
        public string? NombreWeb { get; set; }
        public int? sector_ini { get; set; }


    }
    public class StProcs
    {
        public string? id { get; set; }
        public string? nombre { get; set; }
        public string? definicion { get; set; }
    }
    public class Disp
    {
        public int valido { get; set; }
        
    }

    public class Usuarios
    {
        public int idUsuario { get; set; }
        public string? nombre { get; set; }
        public string? alias { get; set; }
        public string? password { get; set; }
        public int idGrupo { get; set; }
        

    }

    public class NoticiasMozos
    {
        public int idMozo { get; set; }
        public string? descNoticia { get; set; }

    }
}
