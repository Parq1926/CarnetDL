using Microsoft.EntityFrameworkCore;
using TiposUsuarioSRV5.Entities;

namespace TiposUsuarioSRV5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TipoUsuario> TiposUsuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TipoUsuario>(entity =>
            {
                entity.ToTable("TIPOUSUARIO", "PameRojas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Nombre).HasColumnName("NOMBRE").IsRequired().HasMaxLength(50);
            });
        }
    }
}