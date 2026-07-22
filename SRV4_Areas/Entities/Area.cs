namespace SRV4_Areas.Entities;

public class AreaTrabajo
{
    public int ID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int InstitucionID { get; set; }
    public string InstitucionNombre { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}