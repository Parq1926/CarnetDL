using System.ComponentModel.DataAnnotations;

namespace SRV13_Fotografia.Entities
{
    // La fotografia se almacena en la columna dbo.Usuario.Fotografia (VARBINARY(MAX))
    // y se expone en formato Base 64
    // Resultado que devuelven las operaciones
    public class FotografiaUsuario
    {
        public int UsuarioId { get; set; }
        public string? FotografiaBase64 { get; set; }
    }

    public class ActualizarFotografiaRequest
    {
        [Required(ErrorMessage = "El identificador del usuario es requerido")]
        public int? UsuarioId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La fotografia en Base 64 es requerida")]
        public string FotografiaBase64 { get; set; } = null!;
    }
}
