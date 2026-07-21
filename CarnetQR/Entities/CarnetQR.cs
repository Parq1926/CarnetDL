namespace SRV14_CarnetQR.Entities
{
    public class CarnetQRData
    {
        public string NombreCompleto { get; set; } = null!;
        public string Identificacion { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;
        public List<string> CarrerasOAreas { get; set; } = new();
        public string Institucion { get; set; } = null!;
        public DateTime FechaVencimiento { get; set; }
    }

    public class CarnetQR
    {
        public int Id { get; set; }
        public string UsuarioIdentificacion { get; set; } = null!;
        public string QrBase64 { get; set; } = null!;
        public DateTime FechaGeneracion { get; set; }
    }

    public class CarnetQRResponse
    {
        public string UsuarioIdentificacion { get; set; } = null!;
        public string QrBase64 { get; set; } = null!;
        public DateTime FechaGeneracion { get; set; }
        public CarnetQRData Datos { get; set; } = null!;
    }
}
