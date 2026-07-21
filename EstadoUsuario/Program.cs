using Microsoft.OpenApi.Models;
using SRV12_EstadoUsuario;
using SRV12_EstadoUsuario.Auth;
using SRV12_EstadoUsuario.Repository;
using SRV12_EstadoUsuario.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SRV12 Estado Usuario", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT obtenido del SRV1"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

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

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("ReactDev");
app.MapEstadoUsuarioEndpoints();
app.Run();
