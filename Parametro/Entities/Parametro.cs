using System.ComponentModel.DataAnnotations;

namespace SRV15_Parametro.Entities
{
    public class Parametro
    {
        public string Id { get; set; } = null!;
        public string Valor { get; set; } = null!;
    }

    public class ParametroRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El identificador es requerido")]
        public string Id { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El valor es requerido")]
        public string Valor { get; set; } = null!;
    }
}
