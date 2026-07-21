using System.ComponentModel.DataAnnotations;

namespace SRV13_Fotografia.Entities
{
    public class FotografiaUsuario
    {
        public int Id { get; set; }
        public string UsuarioIdentificacion { get; set; } = null!;
        public string FotografiaBase64 { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }

    public class ActualizarFotografiaRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "La identificación del usuario es requerida")]
        public string UsuarioIdentificacion { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "La fotografía en Base64 es requerida")]
        public string FotografiaBase64 { get; set; } = null!;
    }
}
