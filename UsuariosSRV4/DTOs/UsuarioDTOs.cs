namespace UsuariosSRV4.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public bool Bloqueado { get; set; }
        public int IntentosFallidos { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? FotografiaBase64 { get; set; }
        public List<string> Telefonos { get; set; } = new();
        public List<int> CarrerasIds { get; set; } = new();
        public List<CarreraDto> Carreras { get; set; } = new();
        public List<int> AreasIds { get; set; } = new();
        public List<AreaDto> Areas { get; set; } = new();
    }

    // ✅ CrearUsuarioDto - Para crear un nuevo usuario
    public class CrearUsuarioDto
    {
        public string Email { get; set; } = string.Empty;
        public int TipoIdentificacionId { get; set; }
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int TipoUsuarioId { get; set; }
        public List<string> Telefonos { get; set; } = new();
        public List<int> CarrerasIds { get; set; } = new();
        public List<int> AreasIds { get; set; } = new();
    }

    // ✅ ActualizarUsuarioDto - Para actualizar un usuario existente
    public class ActualizarUsuarioDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public int TipoIdentificacionId { get; set; }
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Contrasena { get; set; }
        public int TipoUsuarioId { get; set; }
        public bool Activo { get; set; }
        public List<string> Telefonos { get; set; } = new();
        public List<int> CarrerasIds { get; set; } = new();
        public List<int> AreasIds { get; set; } = new();
    }

    // ✅ FiltroUsuarioDto - Para filtrar usuarios
    public class FiltroUsuarioDto
    {
        public string? Identificacion { get; set; }
        public string? Nombre { get; set; }
        public string? TipoUsuario { get; set; }
        public bool? Activo { get; set; }
    }
}