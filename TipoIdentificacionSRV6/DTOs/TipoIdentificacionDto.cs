using System.ComponentModel.DataAnnotations;

namespace TipoIdentificacionSRV6.DTOs
{
    public class TipoIdentificacionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class TipoIdentificacionCreateDto
    {
        [Required(ErrorMessage = "El nombre del tipo de identificacion es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoIdentificacionUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de identificacion es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }
}