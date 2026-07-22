using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipoIdentificacionSRV6.Entities
{
    [Table("TIPOIDENTIFICACION", Schema = "PameRojas")]
    public class TipoIdentificacion
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; } = string.Empty;
    }
}