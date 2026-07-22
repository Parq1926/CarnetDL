using Microsoft.EntityFrameworkCore;
using UsuariosSRV4.Data;
using UsuariosSRV4.DTOs;
using UsuariosSRV4.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// ✅ Agregar servicios
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseRouting();

// ✅ ENDPOINT PRINCIPAL - VALIDAR CREDENCIALES
app.MapPost("/api/Usuarios/validar-credenciales", async (
    [FromBody] ValidarCredencialesRequest request,
    IUsuarioService service) =>
{
    Console.WriteLine($"📡 Validando credenciales: {request.Email}");

    var (ok, error, data) = await service.ValidarCredencialesAsync(
        request.Email,
        request.Password,
        request.Tipo);

    if (!ok || data == null)
    {
        Console.WriteLine($"❌ Error: {error}");
        return Results.NotFound(new { message = error ?? "Credenciales inválidas" });
    }

    Console.WriteLine($"✅ Usuario validado: {data.Email}");
    return Results.Ok(data);
});

// ✅ ENDPOINT - LISTAR USUARIOS
app.MapGet("/api/Usuarios", async (IUsuarioService service) =>
{
    var (ok, error, data) = await service.GetAllAsync();
    if (!ok) return Results.BadRequest(new { error });
    return Results.Ok(data);
});

// ✅ ENDPOINT - OBTENER USUARIO POR ID
app.MapGet("/api/Usuarios/{id}", async (int id, IUsuarioService service) =>
{
    var (ok, error, data) = await service.GetByIdAsync(id);
    if (!ok) return Results.NotFound(new { error });
    return Results.Ok(data);
});

app.MapRazorPages();

app.Run();