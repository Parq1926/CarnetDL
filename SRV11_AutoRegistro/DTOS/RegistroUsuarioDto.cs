namespace SRV11_AutoRegistro.DTOs;

public class RegistroUsuarioDto
{
    public int TipoUsuarioId { get; set; }

    public int TipoIdentificacionId { get; set; }

    public string NumeroIdentificacion { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Contrasena { get; set; } = string.Empty;

    public int RolId { get; set; }

    public List<int> Instituciones { get; set; } = new List<int>();

    public List<int> CarrerasAsociadas { get; set; } = new List<int>();

    public List<int> AreasAsociadas { get; set; } = new List<int>();

    public List<string> Telefonos { get; set; } = new List<string>();
}