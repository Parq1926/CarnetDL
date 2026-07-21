using Microsoft.EntityFrameworkCore;
using UsuariosSRV4.Entities;

namespace UsuariosSRV4.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Telefono> Telefonos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIO", "PameRojas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Email).HasColumnName("EMAIL").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Contrasena).HasColumnName("CONTRASENA").IsRequired().HasMaxLength(255);
                entity.Property(e => e.TipoIdentificacionId).HasColumnName("TIPO_IDENTIFICACION_ID");
                entity.Property(e => e.NumeroIdentificacion).HasColumnName("NUMERO_IDENTIFICACION").HasMaxLength(50);
                entity.Property(e => e.NombreCompleto).HasColumnName("NOMBRE_COMPLETO").IsRequired().HasMaxLength(200);
                entity.Property(e => e.TipoUsuarioId).HasColumnName("TIPO_USUARIO_ID");
                entity.Property(e => e.Activo).HasColumnName("ACTIVO");
                entity.Property(e => e.FechaCreacion).HasColumnName("FECHA_CREACION");
                entity.Property(e => e.FechaModificacion).HasColumnName("FECHA_MODIFICACION");
                entity.Property(e => e.Bloqueado).HasColumnName("BLOQUEADO");
                entity.Property(e => e.IntentosFallidos).HasColumnName("INTENTOS_FALLIDOS");

                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasMany(e => e.Telefonos)
                      .WithOne(e => e.Usuario)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Telefono>(entity =>
            {
                entity.ToTable("TELEFONO", "PameRojas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");
                entity.Property(e => e.Numero).HasColumnName("NUMERO").IsRequired().HasMaxLength(20);
                entity.Property(e => e.Activo).HasColumnName("ACTIVO");
                // ✅ ELIMINAR ESTA LÍNEA
                // entity.Property(e => e.FechaCreacion).HasColumnName("FECHA_CREACION");
            });
        }
    }
}