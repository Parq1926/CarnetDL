namespace AreasSRV7.DTOs
{
    public class AreaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
        public string InstitucionNombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }

    public class AreaCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
        public string InstitucionNombre { get; set; } = string.Empty;
    }

    public class AreaUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
        public string InstitucionNombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}