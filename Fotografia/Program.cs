using SRV13_Fotografia;
using SRV13_Fotografia.Auth;
using SRV13_Fotografia.Repository;
using SRV13_Fotografia.Services;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
app.UseCors("ReactDev");
app.MapFotografiaEndpoints();
app.Run();
