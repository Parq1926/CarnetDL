using Microsoft.EntityFrameworkCore;
using TiposUsuarioSRV5.Data;
using TiposUsuarioSRV5.Endpoints;
using TiposUsuarioSRV5.Services;

var builder = WebApplication.CreateBuilder(args);

//Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Registrar ApiClient
builder.Services.AddHttpClient<ITipoUsuarioApiClient, TipoUsuarioApiClient>(client =>
{
    var url = builder.Configuration["Services:TiposUsuarioApi"] ?? "https://localhost:7020";
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Servicios
builder.Services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();

//CORS
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

// MAPEAR ENDPOINTS
app.MapTipoUsuarioEndpoints();

app.Run();