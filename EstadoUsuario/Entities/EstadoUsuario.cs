using System.ComponentModel.DataAnnotations;

namespace SRV12_EstadoUsuario.Entities
{
    public class EstadoUsuario
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }

    public class UsuarioEstado
    {
        public int Id { get; set; }
        public string UsuarioIdentificacion { get; set; } = null!;
        public int EstadoUsuarioId { get; set; }
        public DateTime FechaModificacion { get; set; }
    }

    public class CambioEstadoRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "La identificación del usuario es requerida")]
        public string UsuarioIdentificacion { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El código del estado es requerido")]
        public string CodigoEstado { get; set; } = null!;
    }
}
