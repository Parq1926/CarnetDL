using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosSRV4.Entities
{
    [Table("UsuarioTelefono", Schema = "dbo")]
    public class UsuarioTelefono
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("UsuarioId")]
        public int UsuarioId { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; } = string.Empty;

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}