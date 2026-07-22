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

// ✅ HttpClient para LoginSRV1 (API)
var loginUrl = builder.Configuration["Services:LoginSRV1"] ?? "https://localhost:7019";

builder.Services.AddHttpClient("Login", c =>
{
    c.BaseAddress = new Uri(loginUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.Timeout = TimeSpan.FromSeconds(30);
});

// ✅ HttpClient para otros microservicios
var baseUrl = builder.Configuration["MicroservicioBase"] ?? "https://tiusr22pl.cuc-carrera-ti.ac.cr";

builder.Services.AddHttpClient("EstadoUsuario", c =>
{
    c.BaseAddress = new Uri($"{baseUrl}/EstadoUsuario/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("CarnetQR", c =>
{
    c.BaseAddress = new Uri($"{baseUrl}/CarnetQR/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("Fotografia", c =>
{
    c.BaseAddress = new Uri($"{baseUrl}/Fotografia/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("Parametro", c =>
{
    c.BaseAddress = new Uri($"{baseUrl}/Parametros/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.Timeout = TimeSpan.FromSeconds(30);
});

// ✅ REGISTRAR TODOS LOS SERVICIOS
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEstadoUsuarioService, EstadoUsuarioService>();
builder.Services.AddScoped<ICarnetQRService, CarnetQRService>();
builder.Services.AddScoped<IFotografiaService, FotografiaService>();
builder.Services.AddScoped<IParametroService, ParametroService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// ✅ Endpoint para recibir token desde LoginSRV1
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

// ✅ Endpoint de Login (consume LoginSRV1)
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

app.MapRazorPages();

// ✅ Redirigir raíz a /Login (página de CarnetDigitalWeb)
app.MapGet("/", async context =>
{
    context.Response.Redirect("/Login");
    await Task.CompletedTask;
});

app.Run();