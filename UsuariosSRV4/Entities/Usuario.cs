using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsuariosSRV4.Entities
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int TipoUsuarioId { get; set; }
        public int EstadoId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public int TipoIdentificacionId { get; set; }
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public int RolId { get; set; }

        // ✅ Cambiar de string a byte[] para que coincida con varbinary en la BD
        public byte[]? Fotografia { get; set; }  // ← CORREGIDO

        public bool Confirmado { get; set; }
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("TipoUsuarioId")]
        public virtual TipoUsuario? TipoUsuario { get; set; }

        [ForeignKey("EstadoId")]
        public virtual EstadoUsuario? Estado { get; set; }

        [ForeignKey("TipoIdentificacionId")]
        public virtual TipoIdentificacion? TipoIdentificacion { get; set; }

        [ForeignKey("RolId")]
        public virtual Rol? Rol { get; set; }

        public virtual ICollection<UsuarioTelefono> Telefonos { get; set; } = new List<UsuarioTelefono>();
        public virtual ICollection<UsuarioArea> Areas { get; set; } = new List<UsuarioArea>();
        public virtual ICollection<UsuarioCarrera> Carreras { get; set; } = new List<UsuarioCarrera>();
        public virtual ICollection<UsuarioInstitucion> Instituciones { get; set; } = new List<UsuarioInstitucion>();
    }
}