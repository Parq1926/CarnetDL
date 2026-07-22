using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosSRV4.Entities
{
    [Table("EstadoUsuario", Schema = "dbo")]
    public class EstadoUsuario
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;
    }
}