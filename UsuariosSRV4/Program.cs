using Microsoft.EntityFrameworkCore;
using UsuariosSRV4.Data;
using UsuariosSRV4.Endpoints;
using UsuariosSRV4.Services;

var builder = WebApplication.CreateBuilder(args);


//  Razor Pages (UI)
builder.Services.AddRazorPages();

//  Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  HttpClient para Áreas (SRV4_Areas)
builder.Services.AddHttpClient<IAreaApiClient, AreaApiClient>(client =>
{
    var url = builder.Configuration["Services:AreasSRV4"] ?? "http://localhost:5202";
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

//  HttpClient para Tipos de Identificación (TipoIdentificacionSRV6)
builder.Services.AddHttpClient<ITipoIdentificacionApiClient, TipoIdentificacionApiClient>(client =>
{
    var url = builder.Configuration["Services:TipoIdentificacionSRV6"] ?? "https://localhost:7021";
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// HttpClient para Carreras (SRV3_Carreras)
builder.Services.AddHttpClient<ICarreraApiClient, CarreraApiClient>(client =>
{
    var url = builder.Configuration["Services:CarrerasSRV3"] ?? "http://localhost:5280";
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

//  HttpClient para Tipos de Usuario (TiposUsuarioSRV5)
builder.Services.AddHttpClient<ITipoUsuarioApiClient, TipoUsuarioApiClient>(client =>
{
    var url = builder.Configuration["Services:TiposUsuarioSRV5"] ?? "https://localhost:7020";
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

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
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapUsuarioEndpoints();

app.Run();