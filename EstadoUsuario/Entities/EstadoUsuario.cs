using System.ComponentModel.DataAnnotations;

namespace SRV12_EstadoUsuario.Entities
{
    // Catalogo EstadoUsuario de la base tiusr22pl_userDB
    public class EstadoUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }

    // Resultado que devuelve la operacion de cambio de estado
    public class UsuarioEstado
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public int EstadoId { get; set; }
        public string Estado { get; set; } = null!;
    }

    public class CambioEstadoRequest
    {
        [Required(ErrorMessage = "El identificador del usuario es requerido")]
        public int? UsuarioId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El codigo del estado es requerido")]
        public string Estado { get; set; } = null!;
    }
}
