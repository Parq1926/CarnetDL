namespace SRV3_Carreras.Entities;

public class Carrera
{
    public int ID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public int InstitucionID { get; set; }
    public string InstitucionNombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}