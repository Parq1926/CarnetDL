namespace UsuariosSRV4.DTOs
{
    public class TipoUsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class TipoUsuarioCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoUsuarioUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}