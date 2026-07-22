using Microsoft.EntityFrameworkCore;
using TipoIdentificacionSRV6.Data;
using TipoIdentificacionSRV6.Endpoints;
using TipoIdentificacionSRV6.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Agregar Razor Pages
builder.Services.AddRazorPages();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ REGISTRAR HttpClient para TipoIdentificacionApiClient
builder.Services.AddHttpClient<ITipoIdentificacionApiClient, TipoIdentificacionApiClient>(client =>
{
    var url = builder.Configuration["Services:TipoIdentificacionApi"] ?? "https://localhost:7021";
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// ✅ Services
builder.Services.AddScoped<ITipoIdentificacionService, TipoIdentificacionService>();

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
app.MapTipoIdentificacionEndpoints();

app.Run();