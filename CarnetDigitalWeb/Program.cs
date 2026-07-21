using CarnetDigitalWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
});

// Cada microservicio quedo publicado bajo el mismo host, en su propio path
// (sin "api/"), por eso cada HttpClient nombrado apunta a su segmento.
var baseUrl = builder.Configuration["MicroservicioBase"] ?? "https://tiusr22pl.cuc-carrera-ti.ac.cr";

builder.Services.AddHttpClient("EstadoUsuario", c => c.BaseAddress = new Uri($"{baseUrl}/EstadoUsuario/"));
builder.Services.AddHttpClient("CarnetQR", c => c.BaseAddress = new Uri($"{baseUrl}/CarnetQR/"));
builder.Services.AddHttpClient("Fotografia", c => c.BaseAddress = new Uri($"{baseUrl}/Fotografia/"));
builder.Services.AddHttpClient("Parametro", c => c.BaseAddress = new Uri($"{baseUrl}/Parametros/"));

builder.Services.AddScoped<IEstadoUsuarioService, EstadoUsuarioService>();
builder.Services.AddScoped<ICarnetQRService, CarnetQRService>();
builder.Services.AddScoped<IFotografiaService, FotografiaService>();
builder.Services.AddScoped<IParametroService, ParametroService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// El token se escribe manualmente (no hay pantalla de login en este proyecto)
// y se guarda en la sesion del servidor para usarlo en cada llamada saliente.
app.MapPost("/set-token", (HttpContext ctx) =>
{
    var token = ctx.Request.Form["token"].ToString();
    ctx.Session.SetString("Token", token);
    var referer = ctx.Request.Headers.Referer.ToString();
    return Results.Redirect(string.IsNullOrEmpty(referer) ? "/" : referer);
});

app.MapRazorPages();
app.Run();
