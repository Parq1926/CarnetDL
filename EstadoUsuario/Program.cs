using SRV12_EstadoUsuario;
using SRV12_EstadoUsuario.Auth;
using SRV12_EstadoUsuario.Repository;
using SRV12_EstadoUsuario.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDev", policy =>
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddHttpClient<ITokenValidator, TokenValidator>();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<EstadoUsuarioRepository>();
builder.Services.AddScoped<IEstadoUsuarioService, EstadoUsuarioService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("ReactDev");
app.MapEstadoUsuarioEndpoints();
app.Run();
