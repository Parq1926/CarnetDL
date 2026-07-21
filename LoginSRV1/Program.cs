using LoginSRV1.Data;
using LoginSRV1.Endpoints;
using LoginSRV1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

// Database para Refresh Tokens
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Configurar HttpClient para AuthService con BaseAddress
builder.Services.AddHttpClient<IAuthService, AuthService>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var usuariosUrl = configuration["Services:UsuariosSRV4"] ?? "https://localhost:7206";
    client.BaseAddress = new Uri(usuariosUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Services
builder.Services.AddScoped<IAuthService, AuthService>();

// CORS
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
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapLoginEndpoints();

app.Run();