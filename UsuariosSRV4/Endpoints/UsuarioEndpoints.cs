using Microsoft.AspNetCore.Mvc;
using UsuariosSRV4.DTOs;
using UsuariosSRV4.Services;

namespace UsuariosSRV4.Endpoints
{
    public static class UsuariosEndpoints
    {
        public static void MapUsuarioEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Usuarios").WithTags("Usuarios");

            // ✅ GET: /api/Usuarios - Obtener todos (SOLO UNA VEZ)
            group.MapGet("/", async (IUsuarioService service) =>
            {
                try
                {
                    Console.WriteLine("📡 GET /api/Usuarios - Iniciando...");
                    var result = await service.GetAllAsync();
                    Console.WriteLine($"📡 GET /api/Usuarios - Éxito: {result?.Count() ?? 0} usuarios");
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en GET /api/Usuarios: {ex.Message}");
                    Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
                    return Results.Problem($"Error: {ex.Message}", statusCode: 500);
                }
            });

            // ✅ GET: /api/Usuarios/{id} - Obtener por ID
            group.MapGet("/{id}", async (int id, IUsuarioService service) =>
            {
                try
                {
                    Console.WriteLine($"📡 GET /api/Usuarios/{id} - Buscando usuario...");
                    var result = await service.GetByIdAsync(id);
                    if (result == null)
                    {
                        Console.WriteLine($"❌ Usuario no encontrado ID: {id}");
                        return Results.NotFound(new { error = $"Usuario con ID {id} no encontrado" });
                    }
                    Console.WriteLine($"✅ Usuario encontrado: {result.Email}");
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en GET /api/Usuarios/{id}: {ex.Message}");
                    return Results.Problem(ex.Message, statusCode: 500);
                }
            });

            // ✅ POST: /api/Usuarios - Crear usuario
            group.MapPost("/", async ([FromBody] CrearUsuarioDto dto, IUsuarioService service) =>
            {
                try
                {
                    Console.WriteLine($"📝 POST /api/Usuarios - Creando usuario");
                    Console.WriteLine($"   Email: {dto.Email}");
                    Console.WriteLine($"   Contraseña recibida: {(string.IsNullOrEmpty(dto.Contrasena) ? "VACÍA" : "SÍ (" + dto.Contrasena.Length + " caracteres)")}");
                    Console.WriteLine($"   TipoUsuarioId: {dto.TipoUsuarioId}");
                    Console.WriteLine($"   Nombre: {dto.NombreCompleto}");

                    var (ok, error, data) = await service.CreateAsync(dto);
                    if (!ok)
                    {
                        Console.WriteLine($"❌ Error al crear: {error}");
                        return Results.Conflict(new { error = error });
                    }
                    Console.WriteLine($"✅ Usuario creado con ID: {data?.Id}");
                    return Results.Created($"/api/Usuarios/{data?.Id}", data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en POST: {ex.Message}");
                    return Results.Problem(ex.Message, statusCode: 500);
                }
            });

            // ✅ PUT: /api/Usuarios/{id} - Actualizar usuario
            group.MapPut("/{id}", async (int id, [FromBody] ActualizarUsuarioDto dto, IUsuarioService service) =>
            {
                try
                {
                    Console.WriteLine($"📝 PUT /api/Usuarios/{id} - Actualizando usuario...");

                    if (id != dto.Id)
                    {
                        Console.WriteLine($"❌ ID no coincide: ruta={id}, cuerpo={dto.Id}");
                        return Results.BadRequest(new { error = "El ID de la ruta no coincide con el del cuerpo" });
                    }

                    var (ok, error, data) = await service.UpdateAsync(id, dto);
                    if (!ok)
                    {
                        if (error?.Contains("no encontrado") == true)
                        {
                            Console.WriteLine($"❌ Usuario no encontrado ID: {id}");
                            return Results.NotFound(new { error = error });
                        }
                        Console.WriteLine($"❌ Error al actualizar: {error}");
                        return Results.Conflict(new { error = error });
                    }
                    Console.WriteLine($"✅ Usuario actualizado ID: {id}");
                    return Results.Ok(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en PUT: {ex.Message}");
                    return Results.Problem(ex.Message, statusCode: 500);
                }
            });

            // ✅ DELETE: /api/Usuarios/{id} - Eliminar usuario (soft delete)
            group.MapDelete("/{id}", async (int id, IUsuarioService service) =>
            {
                try
                {
                    Console.WriteLine($"🗑️ DELETE /api/Usuarios/{id} - Eliminando usuario...");

                    var (ok, error) = await service.DeleteAsync(id);
                    if (!ok)
                    {
                        Console.WriteLine($"❌ Error al eliminar: {error}");
                        return Results.NotFound(new { error = error });
                    }
                    Console.WriteLine($"✅ Usuario eliminado ID: {id}");
                    return Results.Ok(new { message = "Usuario eliminado correctamente" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en DELETE: {ex.Message}");
                    return Results.Problem(ex.Message, statusCode: 500);
                }
            });

            // ✅ POST: /api/Usuarios/buscar - Buscar con filtros
            group.MapPost("/buscar", async ([FromBody] FiltroUsuarioDto filtro, IUsuarioService service) =>
            {
                try
                {
                    Console.WriteLine($"📡 POST /api/Usuarios/buscar - Aplicando filtros...");
                    var result = await service.GetByFilterAsync(filtro);
                    Console.WriteLine($"✅ Filtro aplicado - {result.Count()} usuarios encontrados");
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en POST /buscar: {ex.Message}");
                    return Results.Problem(ex.Message, statusCode: 500);
                }
            });

            // ✅ PUT: /api/Usuarios/{id}/desbloquear - Desbloquear usuario
            group.MapPut("/{id}/desbloquear", async (int id, IUsuarioService service) =>
            {
                try
                {
                    Console.WriteLine($"🔓 PUT /api/Usuarios/{id}/desbloquear - Desbloqueando usuario...");

                    var result = await service.DesbloquearUsuarioAsync(id);
                    if (!result)
                    {
                        Console.WriteLine($"❌ Usuario no encontrado ID: {id}");
                        return Results.NotFound(new { error = "Usuario no encontrado" });
                    }
                    Console.WriteLine($"✅ Usuario desbloqueado ID: {id}");
                    return Results.Ok(new { message = "Usuario desbloqueado exitosamente" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error en PUT /desbloquear: {ex.Message}");
                    return Results.Problem(ex.Message, statusCode: 500);
                }
            });

            // ✅ POST: /api/Usuarios/validar-credenciales - Validar credenciales (para LoginSRV1)
            group.MapPost("/validar-credenciales", async (
                [FromBody] ValidarCredencialesRequest request,
                IUsuarioService service) =>
            {
                try
                {
                    Console.WriteLine($"=== VALIDANDO CREDENCIALES ===");
                    Console.WriteLine($"Email: {request.Email}");
                    Console.WriteLine($"Tipo: {request.Tipo}");
                    Console.WriteLine($"Password: {request.Password}");

                    var result = await service.ValidarCredencialesAsync(
                        request.Email,
                        request.Password,
                        request.Tipo ?? ""
                    );

                    if (result == null)
                    {
                        Console.WriteLine($"❌ Credenciales inválidas");
                        return Results.NotFound(new { mensaje = "Usuario o contraseña incorrectos" });
                    }

                    Console.WriteLine($"✅ Usuario validado: {result.Email}");
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                    return Results.Problem(ex.Message, statusCode: 500);
                }
            });
        }
    }
}