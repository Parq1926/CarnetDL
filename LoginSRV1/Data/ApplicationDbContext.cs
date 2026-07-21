using LoginSRV1.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginSRV1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("SESION", "PameRojas");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Token).HasColumnName("REFRESH_TOKEN").IsRequired().HasMaxLength(500);
                entity.Property(e => e.UserId).HasColumnName("USUARIO_ID").IsRequired();
                entity.Property(e => e.ExpiresAt).HasColumnName("FECHA_EXPIRACION");
                entity.Property(e => e.CreatedAt).HasColumnName("FECHA_CREACION");
                entity.Property(e => e.IsRevoked).HasColumnName("ACTIVO");
            });
        }
    }
}