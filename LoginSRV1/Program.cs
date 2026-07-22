using LoginSRV1.Data;
using LoginSRV1.Endpoints;
using LoginSRV1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ AuthDB - para Refresh Tokens (SESION)
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDB")));

// ✅ Servicios
builder.Services.AddScoped<IAuthService, AuthService>();

// ✅ HttpClient para llamar a UsuariosSRV4
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    var usuariosUrl = builder.Configuration["Services:UsuariosSRV4"] ?? "https://localhost:7206";
    client.BaseAddress = new Uri(usuariosUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

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

app.MapLoginEndpoints();

app.Run();