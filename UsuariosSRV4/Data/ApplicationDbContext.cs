using Microsoft.EntityFrameworkCore;
using UsuariosSRV4.Entities;

namespace UsuariosSRV4.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoUsuario> TiposUsuario { get; set; }
        public DbSet<TipoIdentificacion> TiposIdentificacion { get; set; }
        public DbSet<EstadoUsuario> EstadosUsuario { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioTelefono> UsuariosTelefonos { get; set; }
        public DbSet<UsuarioArea> UsuariosAreas { get; set; }
        public DbSet<UsuarioCarrera> UsuariosCarreras { get; set; }
        public DbSet<UsuarioInstitucion> UsuariosInstituciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Nombres de tablas (CON LOS NOMBRES REALES DE LA BD)
            modelBuilder.Entity<Usuario>().ToTable("Usuario", "dbo");
            modelBuilder.Entity<TipoUsuario>().ToTable("TipoUsuario", "dbo");
            modelBuilder.Entity<TipoIdentificacion>().ToTable("TipoIdentificacion", "dbo");  // ← CORREGIDO
            modelBuilder.Entity<EstadoUsuario>().ToTable("EstadoUsuario", "dbo");
            modelBuilder.Entity<Rol>().ToTable("Rol", "dbo");
            modelBuilder.Entity<UsuarioTelefono>().ToTable("UsuarioTelefono", "dbo");
            modelBuilder.Entity<UsuarioArea>().ToTable("UsuarioArea", "dbo");
            modelBuilder.Entity<UsuarioCarrera>().ToTable("UsuarioCarera", "dbo");  // ← CORREGIDO (sin la 'r')
            modelBuilder.Entity<UsuarioInstitucion>().ToTable("UsuarioInstitución", "dbo");  // ← CORREGIDO (con tilde)

            // Configurar relaciones
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.TipoUsuario)
                .WithMany()
                .HasForeignKey(u => u.TipoUsuarioId);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Estado)
                .WithMany()
                .HasForeignKey(u => u.EstadoId);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.TipoIdentificacion)
                .WithMany()
                .HasForeignKey(u => u.TipoIdentificacionId);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany()
                .HasForeignKey(u => u.RolId);
        }
    }
}