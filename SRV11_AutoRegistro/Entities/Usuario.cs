namespace SRV11_AutoRegistro.Entities;

public class Usuario
{
    public int ID { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Contrasena { get; set; } = string.Empty;

    public int TipoUsuarioId { get; set; }

    public int TipoIdentificacionId { get; set; }

    public string NumeroIdentificacion { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public List<int> Instituciones { get; set; } = [];

    public List<int> CarrerasAsociadas { get; set; } = [];
    public List<string> Telefonos { get; set; } = [];

    public List<int> AreasAsociadas { get; set; } = [];

    public int RolId { get; set; }

    public bool Confirmado { get; set; }

    public string? TokenConfirmacion { get; set; }

    public DateTime? FechaExpiracion { get; set; }
}