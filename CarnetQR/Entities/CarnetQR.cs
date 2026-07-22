namespace SRV14_CarnetQR.Entities
{
    // Contenido json que se codifica dentro del codigo QR (HU SRV14)
    public class CarnetQRData
    {
        public string NombreCompleto { get; set; } = null!;
        public string Identificacion { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;
        public List<string> CarrerasOAreas { get; set; } = new();
        public string Institucion { get; set; } = null!;
        public DateOnly FechaVencimiento { get; set; }
    }

    // Datos del usuario leidos de tiusr22pl_userDB
    public class UsuarioCarnet
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string NumeroIdentificacion { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;
    }
}
