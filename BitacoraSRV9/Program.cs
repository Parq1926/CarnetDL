using BitacoraSRV9;
using BitacoraSRV9.Repository;
using BitacoraSRV9.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios para Razor Pages
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<BitacoraRepository>();
builder.Services.AddScoped<IBitacoraService, BitacoraService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapBitacoraEndpoints();

app.Run();