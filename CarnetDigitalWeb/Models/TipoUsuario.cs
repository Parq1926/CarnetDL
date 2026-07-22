namespace CarnetDigitalWeb.Models
{
    public class TipoUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoUsuarioCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoUsuarioUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}