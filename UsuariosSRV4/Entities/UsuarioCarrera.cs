using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosSRV4.Entities
{
    [Table("UsuarioCarera")] 
    public class UsuarioCarrera
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string CarreraId { get; set; } = string.Empty;

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}