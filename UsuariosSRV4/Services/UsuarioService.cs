using Microsoft.EntityFrameworkCore;
using UsuariosSRV4.Data;
using UsuariosSRV4.DTOs;
using UsuariosSRV4.Entities;
using UsuariosSRV4.Helpers;

namespace UsuariosSRV4.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _db;
        private readonly IAreaApiClient _areaApiClient;
        private readonly ITipoIdentificacionApiClient _tipoIdentificacionApiClient;
        private readonly ICarreraApiClient _carreraApiClient;
        private readonly ITipoUsuarioApiClient _tipoUsuarioApiClient;

        public UsuarioService(
            ApplicationDbContext db,
            IAreaApiClient areaApiClient,
            ITipoIdentificacionApiClient tipoIdentificacionApiClient,
            ICarreraApiClient carreraApiClient,
            ITipoUsuarioApiClient tipoUsuarioApiClient)
        {
            _db = db;
            _areaApiClient = areaApiClient;
            _tipoIdentificacionApiClient = tipoIdentificacionApiClient;
            _carreraApiClient = carreraApiClient;
            _tipoUsuarioApiClient = tipoUsuarioApiClient;
        }

        private string GetTipoUsuarioNombre(int tipoUsuarioId)
        {
            return tipoUsuarioId switch
            {
                1 => "Estudiante",
                2 => "Funcionario",
                3 => "Administrador",
                _ => "Desconocido"
            };
        }

        // ========================================
        // VALIDAR CREDENCIALES
        // ========================================
        public async Task<ValidarCredencialesResponse?> ValidarCredencialesAsync(string email, string password, string tipo)
        {
            Console.WriteLine($"=== VALIDANDO CREDENCIALES ===");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Tipo: {tipo}");
            Console.WriteLine($"Password recibida: {password}");

            var usuario = await _db.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (usuario == null)
            {
                Console.WriteLine($"❌ Usuario no encontrado: {email}");
                return null;
            }

            Console.WriteLine($"✅ Usuario encontrado: {usuario.Email}");
            Console.WriteLine($"   Hash en DB: {usuario.Contrasena}");
            Console.WriteLine($"   Tipo en DB: {usuario.TipoUsuarioId}");

            if (!usuario.Activo)
            {
                Console.WriteLine($"❌ Usuario inactivo");
                return new ValidarCredencialesResponse
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    NombreCompleto = usuario.NombreCompleto,
                    TipoUsuario = GetTipoUsuarioNombre(usuario.TipoUsuarioId),
                    Activo = usuario.Activo,
                    Bloqueado = usuario.Bloqueado,
                    IntentosFallidos = usuario.IntentosFallidos
                };
            }

            if (usuario.Bloqueado)
            {
                Console.WriteLine($"❌ Usuario bloqueado");
                return new ValidarCredencialesResponse
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    NombreCompleto = usuario.NombreCompleto,
                    TipoUsuario = GetTipoUsuarioNombre(usuario.TipoUsuarioId),
                    Activo = usuario.Activo,
                    Bloqueado = usuario.Bloqueado,
                    IntentosFallidos = usuario.IntentosFallidos
                };
            }

            var isValidPassword = PasswordHelper.VerifyPassword(password, usuario.Contrasena);
            Console.WriteLine($"   ¿Contraseña válida? {isValidPassword}");

            if (isValidPassword)
            {
                Console.WriteLine($"✅ Contraseña correcta");
                usuario.IntentosFallidos = 0;
                await _db.SaveChangesAsync();

                if (!string.IsNullOrEmpty(tipo))
                {
                    var tipoUsuarioNombre = GetTipoUsuarioNombre(usuario.TipoUsuarioId);
                    Console.WriteLine($"   Tipo de usuario convertido: {tipoUsuarioNombre}");
                    Console.WriteLine($"   Tipo esperado: {tipo}");

                    if (tipoUsuarioNombre != tipo)
                    {
                        Console.WriteLine($"❌ Tipo de usuario no coincide");
                        return new ValidarCredencialesResponse
                        {
                            Id = usuario.Id,
                            Email = usuario.Email,
                            NombreCompleto = usuario.NombreCompleto,
                            TipoUsuario = tipoUsuarioNombre,
                            Activo = usuario.Activo,
                            Bloqueado = usuario.Bloqueado,
                            IntentosFallidos = usuario.IntentosFallidos
                        };
                    }
                }

                Console.WriteLine($"✅ Login exitoso para: {usuario.Email}");
                return new ValidarCredencialesResponse
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    NombreCompleto = usuario.NombreCompleto,
                    TipoUsuario = GetTipoUsuarioNombre(usuario.TipoUsuarioId),
                    Activo = usuario.Activo,
                    Bloqueado = usuario.Bloqueado,
                    IntentosFallidos = usuario.IntentosFallidos
                };
            }

            Console.WriteLine($"❌ Contraseña incorrecta");
            usuario.IntentosFallidos++;
            if (usuario.IntentosFallidos >= 5)
            {
                usuario.Bloqueado = true;
                Console.WriteLine($"⚠️ Usuario bloqueado por intentos fallidos");
            }
            await _db.SaveChangesAsync();

            return null;
        }

        // ========================================
        // OBTENER TODOS LOS USUARIOS
        // ========================================
        public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
        {
            try
            {
                Console.WriteLine("📡 GetAllAsync - Iniciando consulta...");

                var usuarios = await _db.Usuarios
                    .Include(u => u.Telefonos)
                    .Where(u => u.Activo)
                    .ToListAsync();

                Console.WriteLine($"📡 GetAllAsync - Usuarios encontrados: {usuarios.Count}");

                var result = usuarios.Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    TipoIdentificacion = u.TipoIdentificacionId.ToString(),
                    NumeroIdentificacion = u.NumeroIdentificacion,
                    NombreCompleto = u.NombreCompleto,
                    TipoUsuario = GetTipoUsuarioNombre(u.TipoUsuarioId),
                    Activo = u.Activo,
                    Bloqueado = u.Bloqueado,
                    IntentosFallidos = u.IntentosFallidos,
                    FechaCreacion = u.FechaCreacion,
                    Telefonos = u.Telefonos.Where(t => t.Activo).Select(t => t.Numero).ToList(),
                    AreasIds = new List<int>(),
                    Areas = new List<AreaDto>()
                }).ToList();

                Console.WriteLine($"📡 GetAllAsync - Resultado mapeado: {result.Count} usuarios");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ GetAllAsync - Error: {ex.Message}");
                throw;
            }
        }

        // ========================================
        // OBTENER USUARIO POR ID
        // ========================================
        public async Task<UsuarioDto?> GetByIdAsync(int id)
        {
            try
            {
                Console.WriteLine($"📡 GetByIdAsync - Buscando usuario ID: {id}");

                var u = await _db.Usuarios
                    .Include(u => u.Telefonos)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (u == null)
                {
                    Console.WriteLine($"❌ Usuario no encontrado ID: {id}");
                    return null;
                }

                Console.WriteLine($"✅ Usuario encontrado: {u.Email}");

                var allAreas = await _areaApiClient.GetAllAsync();

                return new UsuarioDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    TipoIdentificacion = u.TipoIdentificacionId.ToString(),
                    NumeroIdentificacion = u.NumeroIdentificacion,
                    NombreCompleto = u.NombreCompleto,
                    TipoUsuario = GetTipoUsuarioNombre(u.TipoUsuarioId),
                    Activo = u.Activo,
                    Bloqueado = u.Bloqueado,
                    IntentosFallidos = u.IntentosFallidos,
                    FechaCreacion = u.FechaCreacion,
                    Telefonos = u.Telefonos.Where(t => t.Activo).Select(t => t.Numero).ToList(),
                    AreasIds = new List<int>(),
                    Areas = allAreas
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ GetByIdAsync - Error: {ex.Message}");
                throw;
            }
        }

        // ========================================
        // CREAR USUARIO
        // ========================================
        public async Task<(bool ok, string? error, UsuarioDto? data)> CreateAsync(CrearUsuarioDto dto)
        {
            try
            {
                Console.WriteLine($"📝 CreateAsync - Creando usuario");
                Console.WriteLine($"   Email: {dto.Email}");
                Console.WriteLine($"   TipoUsuarioId: {dto.TipoUsuarioId}");
                Console.WriteLine($"   Áreas seleccionadas: {(dto.AreasIds != null ? string.Join(", ", dto.AreasIds) : "Ninguna")}");

                var emailValidation = ValidarEmailPorDominio(dto.Email, dto.TipoUsuarioId);
                if (!emailValidation.IsValid)
                {
                    return (false, emailValidation.Error, null);
                }

                var existe = await _db.Usuarios.AnyAsync(u => u.Email == dto.Email);
                if (existe)
                {
                    return (false, "El email ya está registrado", null);
                }

                var hashedPassword = PasswordHelper.HashPassword(dto.Contrasena);

                var entity = new Usuario
                {
                    Email = dto.Email,
                    Contrasena = hashedPassword,
                    TipoIdentificacionId = dto.TipoIdentificacionId,
                    NumeroIdentificacion = dto.NumeroIdentificacion,
                    NombreCompleto = dto.NombreCompleto,
                    TipoUsuarioId = dto.TipoUsuarioId,
                    Activo = true,
                    Bloqueado = false,
                    IntentosFallidos = 0,
                    FechaCreacion = DateTime.Now
                };

                _db.Usuarios.Add(entity);
                await _db.SaveChangesAsync();

                // Guardar teléfonos
                if (dto.Telefonos != null && dto.Telefonos.Any())
                {
                    foreach (var telefono in dto.Telefonos)
                    {
                        _db.Telefonos.Add(new Telefono
                        {
                            UsuarioId = entity.Id,
                            Numero = telefono,
                            Activo = true
                        });
                    }
                    await _db.SaveChangesAsync();
                }

                Console.WriteLine($"✅ Usuario creado con ID: {entity.Id}");

                var allAreas = await _areaApiClient.GetAllAsync();

                var data = new UsuarioDto
                {
                    Id = entity.Id,
                    Email = entity.Email,
                    TipoIdentificacion = entity.TipoIdentificacionId.ToString(),
                    NumeroIdentificacion = entity.NumeroIdentificacion,
                    NombreCompleto = entity.NombreCompleto,
                    TipoUsuario = GetTipoUsuarioNombre(entity.TipoUsuarioId),
                    Activo = entity.Activo,
                    Bloqueado = entity.Bloqueado,
                    IntentosFallidos = entity.IntentosFallidos,
                    FechaCreacion = entity.FechaCreacion,
                    Telefonos = dto.Telefonos ?? new List<string>(),
                    AreasIds = dto.AreasIds ?? new List<int>(),
                    Areas = allAreas.Where(a => (dto.AreasIds ?? new List<int>()).Contains(a.Id)).ToList()
                };

                return (true, null, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ CreateAsync - Error: {ex.Message}");
                return (false, $"Error al crear usuario: {ex.Message}", null);
            }
        }

        // ========================================
        // ACTUALIZAR USUARIO
        // ========================================
        public async Task<(bool ok, string? error, UsuarioDto? data)> UpdateAsync(int id, ActualizarUsuarioDto dto)
        {
            try
            {
                Console.WriteLine($"📝 UpdateAsync - Actualizando usuario ID: {id}");
                Console.WriteLine($"   Email: {dto.Email}");
                Console.WriteLine($"   TipoUsuarioId: {dto.TipoUsuarioId}");
                Console.WriteLine($"   Áreas seleccionadas: {(dto.AreasIds != null ? string.Join(", ", dto.AreasIds) : "Ninguna")}");

                var entity = await _db.Usuarios
                    .Include(u => u.Telefonos)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (entity == null)
                {
                    return (false, "Usuario no encontrado", null);
                }

                var emailValidation = ValidarEmailPorDominio(dto.Email, dto.TipoUsuarioId);
                if (!emailValidation.IsValid)
                {
                    return (false, emailValidation.Error, null);
                }

                var existe = await _db.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id);
                if (existe)
                {
                    return (false, "El email ya está registrado por otro usuario", null);
                }

                entity.Email = dto.Email;
                entity.TipoIdentificacionId = dto.TipoIdentificacionId;
                entity.NumeroIdentificacion = dto.NumeroIdentificacion;
                entity.NombreCompleto = dto.NombreCompleto;
                entity.TipoUsuarioId = dto.TipoUsuarioId;
                entity.Activo = dto.Activo;
                entity.FechaModificacion = DateTime.Now;

                if (!string.IsNullOrEmpty(dto.Contrasena))
                {
                    entity.Contrasena = PasswordHelper.HashPassword(dto.Contrasena);
                }

                // Actualizar teléfonos
                var telefonosExistentes = await _db.Telefonos.Where(t => t.UsuarioId == id).ToListAsync();
                _db.Telefonos.RemoveRange(telefonosExistentes);

                if (dto.Telefonos != null && dto.Telefonos.Any())
                {
                    foreach (var telefono in dto.Telefonos)
                    {
                        _db.Telefonos.Add(new Telefono
                        {
                            UsuarioId = entity.Id,
                            Numero = telefono,
                            Activo = true
                        });
                    }
                }

                await _db.SaveChangesAsync();

                Console.WriteLine($"✅ Usuario actualizado ID: {entity.Id}");

                var allAreas = await _areaApiClient.GetAllAsync();

                var data = new UsuarioDto
                {
                    Id = entity.Id,
                    Email = entity.Email,
                    TipoIdentificacion = entity.TipoIdentificacionId.ToString(),
                    NumeroIdentificacion = entity.NumeroIdentificacion,
                    NombreCompleto = entity.NombreCompleto,
                    TipoUsuario = GetTipoUsuarioNombre(entity.TipoUsuarioId),
                    Activo = entity.Activo,
                    Bloqueado = entity.Bloqueado,
                    IntentosFallidos = entity.IntentosFallidos,
                    FechaCreacion = entity.FechaCreacion,
                    Telefonos = dto.Telefonos ?? new List<string>(),
                    AreasIds = dto.AreasIds ?? new List<int>(),
                    Areas = allAreas.Where(a => (dto.AreasIds ?? new List<int>()).Contains(a.Id)).ToList()
                };

                return (true, null, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ UpdateAsync - Error: {ex.Message}");
                return (false, $"Error al actualizar usuario: {ex.Message}", null);
            }
        }

        // ========================================
        // ELIMINAR USUARIO (Soft Delete)
        // ========================================
        public async Task<(bool ok, string? error)> DeleteAsync(int id)
        {
            try
            {
                Console.WriteLine($"🗑️ DeleteAsync - Eliminando usuario ID: {id}");

                var entity = await _db.Usuarios.FindAsync(id);
                if (entity == null)
                {
                    Console.WriteLine($"❌ Usuario no encontrado ID: {id}");
                    return (false, "Usuario no encontrado");
                }

                entity.Activo = false;
                entity.FechaModificacion = DateTime.Now;
                await _db.SaveChangesAsync();

                Console.WriteLine($"✅ Usuario eliminado (soft delete) ID: {id}");
                return (true, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ DeleteAsync - Error: {ex.Message}");
                return (false, $"Error al eliminar usuario: {ex.Message}");
            }
        }

        // ========================================
        // DESBLOQUEAR USUARIO
        // ========================================
        public async Task<bool> DesbloquearUsuarioAsync(int id)
        {
            try
            {
                Console.WriteLine($"🔓 DesbloquearUsuarioAsync - ID: {id}");

                var entity = await _db.Usuarios.FindAsync(id);
                if (entity == null)
                {
                    Console.WriteLine($"❌ Usuario no encontrado ID: {id}");
                    return false;
                }

                entity.Bloqueado = false;
                entity.IntentosFallidos = 0;
                entity.FechaModificacion = DateTime.Now;
                await _db.SaveChangesAsync();

                Console.WriteLine($"✅ Usuario desbloqueado ID: {id}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ DesbloquearUsuarioAsync - Error: {ex.Message}");
                return false;
            }
        }

        // ========================================
        // FILTRAR USUARIOS
        // ========================================
        public async Task<IEnumerable<UsuarioDto>> GetByFilterAsync(FiltroUsuarioDto filtro)
        {
            try
            {
                Console.WriteLine($"📡 GetByFilterAsync - Aplicando filtros");
                Console.WriteLine($"   Identificacion: {filtro.Identificacion}");
                Console.WriteLine($"   Nombre: {filtro.Nombre}");
                Console.WriteLine($"   TipoUsuario: {filtro.TipoUsuario}");

                var query = _db.Usuarios
                    .Include(u => u.Telefonos)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(filtro.Identificacion))
                {
                    query = query.Where(u => u.NumeroIdentificacion.Contains(filtro.Identificacion));
                }

                if (!string.IsNullOrEmpty(filtro.Nombre))
                {
                    query = query.Where(u => u.NombreCompleto.Contains(filtro.Nombre));
                }

                if (!string.IsNullOrEmpty(filtro.TipoUsuario))
                {
                    var tipoId = filtro.TipoUsuario switch
                    {
                        "Estudiante" => 1,
                        "Funcionario" => 2,
                        "Administrador" => 3,
                        _ => 0
                    };
                    if (tipoId > 0)
                    {
                        query = query.Where(u => u.TipoUsuarioId == tipoId);
                    }
                }

                var usuarios = await query.ToListAsync();

                var allAreas = await _areaApiClient.GetAllAsync();

                return usuarios.Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    TipoIdentificacion = u.TipoIdentificacionId.ToString(),
                    NumeroIdentificacion = u.NumeroIdentificacion,
                    NombreCompleto = u.NombreCompleto,
                    TipoUsuario = GetTipoUsuarioNombre(u.TipoUsuarioId),
                    Activo = u.Activo,
                    Bloqueado = u.Bloqueado,
                    IntentosFallidos = u.IntentosFallidos,
                    FechaCreacion = u.FechaCreacion,
                    Telefonos = u.Telefonos.Where(t => t.Activo).Select(t => t.Numero).ToList(),
                    AreasIds = new List<int>(),
                    Areas = allAreas
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ GetByFilterAsync - Error: {ex.Message}");
                throw;
            }
        }

        // ========================================
        // MÉTODOS PRIVADOS
        // ========================================

        private (bool IsValid, string? Error) ValidarEmailPorDominio(string email, int tipoUsuarioId)
        {
            var emailParts = email.Split('@');
            var dominio = emailParts.Length > 1 ? emailParts[1].ToLower() : "";

            if (tipoUsuarioId == 1 && dominio != "cuc.cr")
            {
                return (false, "Los estudiantes solo pueden usar el dominio @cuc.cr");
            }

            if ((tipoUsuarioId == 2 || tipoUsuarioId == 3) && dominio != "cuc.ac.cr")
            {
                return (false, "Los funcionarios y administradores solo pueden usar el dominio @cuc.ac.cr");
            }

            return (true, null);
        }
    }
}