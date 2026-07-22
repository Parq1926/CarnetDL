using SRV11_AutoRegistro;
using SRV11_AutoRegistro.Services;
using SRV11_AutoRegistro.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<UsuarioRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<UsuarioCarreraRepository>();
builder.Services.AddScoped<UsuarioAreaRepository>();
builder.Services.AddScoped<UsuarioInstitucionRepository>();
builder.Services.AddScoped<UsuarioTelefonoRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRolService, RolService>();



builder.Services.AddScoped<BitacoraService>();

//builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddHttpClient();

builder.Services.AddHttpClient<IInstitucionService, InstitucionService>();
builder.Services.AddHttpClient<ICarreraService, CarreraService>();
builder.Services.AddHttpClient<IAreaService, AreaService>();
builder.Services.AddHttpClient<ITipoUsuarioService,TipoUsuarioService>();
builder.Services.AddHttpClient<ITipoIdentificacionService,TipoIdentificacionService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.MapAutoRegistroEndpoints();

app.Run();