using Microsoft.EntityFrameworkCore;
using TipoIdentificacionSRV6.Entities;

namespace TipoIdentificacionSRV6.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TipoIdentificacion> TiposIdentificacion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TipoIdentificacion>(entity =>
            {
                entity.ToTable("TIPOIDENTIFICACION", "PameRojas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Nombre).HasColumnName("NOMBRE").IsRequired().HasMaxLength(50);
            });
        }
    }
}