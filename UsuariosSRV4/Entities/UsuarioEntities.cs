using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosSRV4.Entities
{
    [Table("USUARIO", Schema = "PameRojas")]
    public class Usuario
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        [Column("CONTRASENA")]
        public string Contrasena { get; set; } = string.Empty;

        [Column("TIPO_IDENTIFICACION_ID")]
        public int TipoIdentificacionId { get; set; }

        [Column("NUMERO_IDENTIFICACION")]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [Column("NOMBRE_COMPLETO")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Column("TIPO_USUARIO_ID")]
        public int TipoUsuarioId { get; set; }

        [Column("ACTIVO")]
        public bool Activo { get; set; } = true;

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("FECHA_MODIFICACION")]
        public DateTime? FechaModificacion { get; set; }

        [Column("BLOQUEADO")]
        public bool Bloqueado { get; set; }

        [Column("INTENTOS_FALLIDOS")]
        public int IntentosFallidos { get; set; }

        public virtual ICollection<Telefono> Telefonos { get; set; } = new List<Telefono>();
    }

    [Table("TELEFONO", Schema = "PameRojas")]
    public class Telefono
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("USUARIO_ID")]
        public int UsuarioId { get; set; }

        [Column("NUMERO")]
        public string Numero { get; set; } = string.Empty;

        [Column("ACTIVO")]
        public bool Activo { get; set; } = true;

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}