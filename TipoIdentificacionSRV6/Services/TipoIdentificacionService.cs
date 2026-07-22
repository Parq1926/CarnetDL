using Microsoft.EntityFrameworkCore;
using TipoIdentificacionSRV6.Data;
using TipoIdentificacionSRV6.DTOs;
using TipoIdentificacionSRV6.Entities;

namespace TipoIdentificacionSRV6.Services
{
    public class TipoIdentificacionService : ITipoIdentificacionService
    {
        private readonly ApplicationDbContext _context;

        public TipoIdentificacionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoIdentificacionDto>> GetAllAsync()
        {
            var tipos = await _context.TiposIdentificacion
                .OrderBy(t => t.Nombre)
                .ToListAsync();

            return tipos.Select(t => new TipoIdentificacionDto
            {
                Id = t.Id,
                Nombre = t.Nombre
            });
        }

        public async Task<TipoIdentificacionDto?> GetByIdAsync(int id)
        {
            var tipo = await _context.TiposIdentificacion
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tipo == null) return null;

            return new TipoIdentificacionDto
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre
            };
        }

        public async Task<int> CreateAsync(TipoIdentificacionCreateDto dto)
        {
            var exists = await _context.TiposIdentificacion
                .AnyAsync(t => t.Nombre == dto.Nombre);

            if (exists)
            {
                throw new Exception("Ya existe un tipo de identificación con ese nombre");
            }

            var tipo = new TipoIdentificacion
            {
                Nombre = dto.Nombre
            };

            _context.TiposIdentificacion.Add(tipo);
            await _context.SaveChangesAsync();

            return tipo.Id;
        }

        public async Task<int> UpdateAsync(TipoIdentificacionUpdateDto dto)
        {
            var tipo = await _context.TiposIdentificacion
                .FirstOrDefaultAsync(t => t.Id == dto.Id);

            if (tipo == null)
            {
                throw new Exception("Tipo de identificación no encontrado");
            }

            var exists = await _context.TiposIdentificacion
                .AnyAsync(t => t.Nombre == dto.Nombre && t.Id != dto.Id);

            if (exists)
            {
                throw new Exception("Ya existe otro tipo de identificación con ese nombre");
            }

            tipo.Nombre = dto.Nombre;
            await _context.SaveChangesAsync();

            return tipo.Id;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var tipo = await _context.TiposIdentificacion
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tipo == null)
            {
                throw new Exception("Tipo de identificación no encontrado");
            }

            _context.TiposIdentificacion.Remove(tipo);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.TiposIdentificacion.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null)
        {
            var query = _context.TiposIdentificacion.Where(t => t.Nombre == nombre);
            if (excludeId.HasValue)
            {
                query = query.Where(t => t.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }
    }
}