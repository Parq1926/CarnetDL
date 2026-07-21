using Microsoft.OpenApi.Models;
using SRV13_Fotografia;
using SRV13_Fotografia.Auth;
using SRV13_Fotografia.Repository;
using SRV13_Fotografia.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SRV13 Fotografía Usuario", Version = "v1" });
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
builder.Services.AddScoped<FotografiaRepository>();
builder.Services.AddScoped<IFotografiaService, FotografiaService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("ReactDev");
app.MapFotografiaEndpoints();
app.Run();
