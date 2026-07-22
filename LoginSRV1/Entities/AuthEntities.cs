using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginSRV1.Entities
{
    [Table("USUARIO", Schema = "PameRojas")]
    public class Usuario
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        [Column("PASSWORD")]
        public string Password { get; set; } = string.Empty;

        [Column("NOMBRE_COMPLETO")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Column("TIPO_USUARIO_ID")]
        public int TipoUsuarioId { get; set; }

        [Column("TIPO_IDENTIFICACION_ID")]
        public int TipoIdentificacionId { get; set; }

        [Column("NUMERO_IDENTIFICACION")]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [Column("ACTIVO")]
        public bool Activo { get; set; } = true;

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; }

        [Column("FECHA_MODIFICACION")]
        public DateTime? FechaModificacion { get; set; }

        [ForeignKey("TipoUsuarioId")]
        public TipoUsuario? TipoUsuario { get; set; }

        [ForeignKey("TipoIdentificacionId")]
        public TipoIdentificacion? TipoIdentificacion { get; set; }
    }

    [Table("TIPOUSUARIO", Schema = "PameRojas")]
    public class TipoUsuario
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; } = string.Empty;
    }

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