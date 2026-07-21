namespace UsuariosSRV4.DTOs
{
    public class CarreraDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
        public string InstitucionNombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }

    public class CarreraCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
        public string InstitucionNombre { get; set; } = string.Empty;
    }

    public class CarreraUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
        public string InstitucionNombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}