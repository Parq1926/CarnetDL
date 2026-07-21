using SRV3_Carreras;
using SRV3_Carreras.Repository;
using SRV3_Carreras.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar Razor Pages
builder.Services.AddRazorPages();

builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<ICarreraRepository, CarreraRepository>();
builder.Services.AddScoped<ICarreraService, CarreraService>();
builder.Services.AddScoped<IBitacoraService, BitacoraService>();

builder.Services.AddHttpClient<IBitacoraService, BitacoraService>();

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
app.UseStaticFiles();
app.UseRouting();

// ✅ PRIMERO: Map Razor Pages (UI)
app.MapRazorPages();

// ✅ SEGUNDO: Map API Endpoints (con prefijo /api)
app.MapCarreraEndpoints();

// Redirigir raíz a Carreras
app.MapGet("/", async context =>
{
    context.Response.Redirect("/Carreras");
    await Task.CompletedTask;
});

app.Run();