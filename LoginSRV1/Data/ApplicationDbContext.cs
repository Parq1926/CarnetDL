using LoginSRV1.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginSRV1.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("SESION", "PameRojas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");
                entity.Property(e => e.Token).HasColumnName("REFRESH_TOKEN");
                entity.Property(e => e.ExpiresAt).HasColumnName("FECHA_EXPIRACION");
                entity.Property(e => e.IsRevoked).HasColumnName("ACTIVO");
                entity.Property(e => e.CreatedAt).HasColumnName("FECHA_CREACION");
            });
        }
    }
}