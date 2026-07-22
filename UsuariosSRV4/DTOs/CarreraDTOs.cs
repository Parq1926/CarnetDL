namespace UsuariosSRV4.DTOs
{
    public class CarreraDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Facultad { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
        public bool Activo { get; set; }
    }

    public class CarreraCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Facultad { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
    }

    public class CarreraUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Facultad { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
        public bool Activo { get; set; }
    }
}