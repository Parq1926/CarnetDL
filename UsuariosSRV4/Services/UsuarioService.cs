using Microsoft.EntityFrameworkCore;
using UsuariosSRV4.Data;
using UsuariosSRV4.DTOs;
using UsuariosSRV4.Entities;

namespace UsuariosSRV4.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool ok, string? error, IEnumerable<UsuarioDto>? data)> GetAllAsync()
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Include(u => u.TipoUsuario)
                    .Include(u => u.Estado)
                    .Include(u => u.TipoIdentificacion)
                    .Include(u => u.Telefonos)
                    .Where(u => u.EstadoId == 1)
                    .ToListAsync();

                var result = usuarios.Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    TipoIdentificacion = u.TipoIdentificacion?.Nombre ?? "",
                    NumeroIdentificacion = u.NumeroIdentificacion,
                    NombreCompleto = u.NombreCompleto,
                    TipoUsuario = u.TipoUsuario?.Nombre ?? "",
                    Activo = u.EstadoId == 1,
                    Bloqueado = false,
                    IntentosFallidos = 0,
                    FechaCreacion = u.FechaCreacion,
                    FotografiaBase64 = u.Fotografia != null ? Convert.ToBase64String(u.Fotografia) : null,
                    Telefonos = u.Telefonos?.Select(t => t.Telefono).ToList() ?? new List<string>()
                });

                return (true, null, result);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener usuarios: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string? error, UsuarioDto? data)> GetByIdAsync(int id)
        {
            try
            {
                var u = await _context.Usuarios
                    .Include(u => u.TipoUsuario)
                    .Include(u => u.Estado)
                    .Include(u => u.TipoIdentificacion)
                    .Include(u => u.Telefonos)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (u == null)
                {
                    return (false, "Usuario no encontrado", null);
                }

                var result = new UsuarioDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    TipoIdentificacion = u.TipoIdentificacion?.Nombre ?? "",
                    NumeroIdentificacion = u.NumeroIdentificacion,
                    NombreCompleto = u.NombreCompleto,
                    TipoUsuario = u.TipoUsuario?.Nombre ?? "",
                    Activo = u.EstadoId == 1,
                    Bloqueado = false,
                    IntentosFallidos = 0,
                    FechaCreacion = u.FechaCreacion,
                    FotografiaBase64 = u.Fotografia != null ? Convert.ToBase64String(u.Fotografia) : null,
                    Telefonos = u.Telefonos?.Select(t => t.Telefono).ToList() ?? new List<string>()
                };

                return (true, null, result);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener usuario: {ex.Message}", null);
            }
        }

        // ✅ CREATE - Implementación correcta
        public async Task<(bool ok, string? error, UsuarioDto? data)> CreateAsync(CrearUsuarioDto dto)
        {
            try
            {
                // Verificar si el email ya existe
                var exists = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
                if (exists)
                {
                    return (false, "El email ya está registrado", null);
                }

                var usuario = new Usuario
                {
                    Email = dto.Email,
                    Contrasena = dto.Contrasena,
                    TipoUsuarioId = dto.TipoUsuarioId,
                    EstadoId = 1, // Activo por defecto
                    NombreCompleto = dto.NombreCompleto,
                    TipoIdentificacionId = dto.TipoIdentificacionId,
                    NumeroIdentificacion = dto.NumeroIdentificacion,
                    RolId = 1, // Rol por defecto
                    Confirmado = true,
                    FechaCreacion = DateTime.Now
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Agregar teléfonos
                foreach (var telefono in dto.Telefonos)
                {
                    if (!string.IsNullOrWhiteSpace(telefono))
                    {
                        _context.UsuariosTelefonos.Add(new UsuarioTelefono
                        {
                            UsuarioId = usuario.Id,
                            Telefono = telefono
                        });
                    }
                }

                await _context.SaveChangesAsync();

                // Obtener el usuario creado
                var (ok, _, data) = await GetByIdAsync(usuario.Id);
                return (ok, null, data);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear usuario: {ex.Message}", null);
            }
        }

        // ✅ UPDATE - Implementación correcta
        public async Task<(bool ok, string? error, UsuarioDto? data)> UpdateAsync(int id, ActualizarUsuarioDto dto)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Telefonos)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    return (false, "Usuario no encontrado", null);
                }

                usuario.Email = dto.Email;
                usuario.TipoIdentificacionId = dto.TipoIdentificacionId;
                usuario.NumeroIdentificacion = dto.NumeroIdentificacion;
                usuario.NombreCompleto = dto.NombreCompleto;
                usuario.TipoUsuarioId = dto.TipoUsuarioId;
                usuario.EstadoId = dto.Activo ? 1 : 2; // 1=Activo, 2=Inactivo

                if (!string.IsNullOrWhiteSpace(dto.Contrasena))
                {
                    usuario.Contrasena = dto.Contrasena;
                }

                // Actualizar teléfonos
                var telefonosActuales = usuario.Telefonos.ToList();
                foreach (var tel in telefonosActuales)
                {
                    _context.UsuariosTelefonos.Remove(tel);
                }

                foreach (var telefono in dto.Telefonos)
                {
                    if (!string.IsNullOrWhiteSpace(telefono))
                    {
                        _context.UsuariosTelefonos.Add(new UsuarioTelefono
                        {
                            UsuarioId = usuario.Id,
                            Telefono = telefono
                        });
                    }
                }

                await _context.SaveChangesAsync();

                var (ok, _, data) = await GetByIdAsync(id);
                return (ok, null, data);
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar usuario: {ex.Message}", null);
            }
        }

        // ✅ DELETE - Implementación correcta
        public async Task<(bool ok, string? error)> DeleteAsync(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return (false, "Usuario no encontrado");
                }

                usuario.EstadoId = 2; // Inactivo (borrado lógico)
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar usuario: {ex.Message}");
            }
        }

        // ✅ VALIDAR CREDENCIALES
        public async Task<(bool ok, string? error, ValidarCredencialesResponse? data)> ValidarCredencialesAsync(string email, string password, string? tipo = null)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.TipoUsuario)
                    .Include(u => u.Estado)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (usuario == null)
                {
                    return (false, "Usuario no encontrado", null);
                }

                if (usuario.Contrasena != password)
                {
                    return (false, "Contraseña incorrecta", null);
                }

                if (usuario.EstadoId != 1)
                {
                    var estado = usuario.Estado?.Nombre ?? "Inactivo";
                    return (false, $"Usuario {estado}", null);
                }

                if (!string.IsNullOrEmpty(tipo))
                {
                    var tipoUsuario = usuario.TipoUsuario?.Nombre ?? "";
                    if (!tipoUsuario.Equals(tipo, StringComparison.OrdinalIgnoreCase))
                    {
                        return (false, "Tipo de usuario no coincide", null);
                    }
                }

                var result = new ValidarCredencialesResponse
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    NombreCompleto = usuario.NombreCompleto,
                    TipoUsuario = usuario.TipoUsuario?.Nombre ?? "",
                    Activo = usuario.EstadoId == 1,
                    Bloqueado = false,
                    IntentosFallidos = 0,
                    TipoUsuarioId = usuario.TipoUsuarioId,
                    RolId = usuario.RolId
                };

                return (true, null, result);
            }
            catch (Exception ex)
            {
                return (false, $"Error al validar credenciales: {ex.Message}", null);
            }
        }
    }
}