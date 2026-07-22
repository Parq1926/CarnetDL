using CarnetDigitalWeb.Models;
using CarnetDigitalWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurar HttpClient para LoginSRV1
var loginUrl = builder.Configuration["Services:LoginSRV1"] ?? "https://localhost:7019";

builder.Services.AddHttpClient("Login", c =>
{
    c.BaseAddress = new Uri(loginUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.Timeout = TimeSpan.FromSeconds(30);
});

// Agregar HttpClient para TipoIdentificacionSRV6
var tipoIdentificacionUrl = builder.Configuration["Services:TipoIdentificacionSRV6"] ?? "https://localhost:7021";

// HttpClient para TiposUsuarioSRV5
var tipoUsuarioUrl = builder.Configuration["Services:TiposUsuarioSRV5"] ?? "https://localhost:7020";

builder.Services.AddHttpClient("TipoUsuario", c =>
{
    c.BaseAddress = new Uri(tipoUsuarioUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.Timeout = TimeSpan.FromSeconds(30);
});

// ✅ Registrar servicio
builder.Services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();

builder.Services.AddHttpClient("TipoIdentificacion", c =>
{
    c.BaseAddress = new Uri(tipoIdentificacionUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.Timeout = TimeSpan.FromSeconds(30);
});

// ✅ Registrar servicio
builder.Services.AddScoped<ITipoIdentificacionService, TipoIdentificacionService>();

// ✅ Servicios
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEstadoUsuarioService, EstadoUsuarioService>();
builder.Services.AddScoped<ICarnetQRService, CarnetQRService>();
builder.Services.AddScoped<IFotografiaService, FotografiaService>();
builder.Services.AddScoped<IParametroService, ParametroService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapGet("/set-token", (HttpContext ctx) =>
{
    var token = ctx.Request.Query["token"].ToString();
    var returnUrl = ctx.Request.Query["returnUrl"].ToString();

    if (!string.IsNullOrEmpty(token))
    {
        ctx.Session.SetString("Token", token);
    }

    var redirectUrl = string.IsNullOrEmpty(returnUrl) ? "/EstadoUsuario" : returnUrl;
    return Results.Redirect(redirectUrl);
});

app.MapPost("/api/login", async (LoginRequest request, ILoginService loginService) =>
{
    var result = await loginService.LoginAsync(request);
    return Results.Ok(result);
});

app.MapPost("/api/logout", (HttpContext ctx) =>
{
    ctx.Session.Remove("Token");
    return Results.Ok(new { success = true, message = "Sesión cerrada" });
});

// ✅ Endpoint de configuración para el frontend
app.MapGet("/api/config", (IConfiguration config) =>
{
    var services = new Dictionary<string, string>();
    var servicesSection = config.GetSection("Services");

    foreach (var child in servicesSection.GetChildren())
    {
        services[child.Key] = child.Value ?? string.Empty;
    }

    return Results.Ok(new { Services = services });
});

app.MapRazorPages();

app.MapGet("/", async context =>
{
    var token = context.Session.GetString("Token");
    if (!string.IsNullOrEmpty(token))
    {
        context.Response.Redirect("/EstadoUsuario");
        return;
    }
    context.Response.Redirect("/Login");
    await Task.CompletedTask;
});

app.Run();