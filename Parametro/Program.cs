using SRV15_Parametro;
using SRV15_Parametro.Auth;
using SRV15_Parametro.Repository;
using SRV15_Parametro.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDev", policy =>
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddHttpClient<ITokenValidator, TokenValidator>();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<ParametroRepository>();
builder.Services.AddScoped<IParametroService, ParametroService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("ReactDev");
app.MapParametroEndpoints();
app.Run();
