namespace UsuariosSRV4.DTOs
{
    public class TipoIdentificacionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class TipoIdentificacionCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoIdentificacionUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}