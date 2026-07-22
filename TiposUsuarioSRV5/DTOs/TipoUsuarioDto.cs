using System.ComponentModel.DataAnnotations;

namespace TiposUsuarioSRV5.DTOs
{
    public class TipoUsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    public class TipoUsuarioCreateDto
    {
        [Required(ErrorMessage = "El nombre del tipo de usuario es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoUsuarioUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de usuario es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }
}