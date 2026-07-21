using Microsoft.EntityFrameworkCore;
using TiposUsuarioSRV5.Data;
using TiposUsuarioSRV5.Endpoints;
using TiposUsuarioSRV5.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Agregar Razor Pages
builder.Services.AddRazorPages();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ REGISTRAR HttpClient para TipoUsuarioApiClient
builder.Services.AddHttpClient<ITipoUsuarioApiClient, TipoUsuarioApiClient>(client =>
{
    var url = builder.Configuration["Services:TipoUsuarioApi"] ?? "https://localhost:7020";
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// ✅ Services
builder.Services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();

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
app.MapTipoUsuarioEndpoints();

app.Run();