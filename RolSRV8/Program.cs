using RolSRV8;
using RolSRV8.Repository;
using RolSRV8.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<RolRepository>();

builder.Services.AddScoped<IRolService, RolService>();

builder.Services.AddHttpClient<IBitacoraClient, BitacoraClient>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.MapRolEndpoints();

app.Run();