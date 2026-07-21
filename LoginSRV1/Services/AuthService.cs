using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LoginSRV1.Data;
using LoginSRV1.DTOs;
using LoginSRV1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LoginSRV1.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ApplicationDbContext db,
            HttpClient http,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _db = db;
            _http = http;
            _configuration = configuration;
            _logger = logger;

            if (_http.BaseAddress == null)
            {
                var usuariosUrl = _configuration["Services:UsuariosSRV4"] ?? "https://localhost:7206";
                _http.BaseAddress = new Uri(usuariosUrl);
                _logger.LogInformation($"BaseAddress configurado en: {_http.BaseAddress}");
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                _logger.LogInformation("=== INICIO LOGIN ===");
                _logger.LogInformation($"Email: {request.Email}");
                _logger.LogInformation($"TipoUsuario: {request.TipoUsuario}");

                var requestData = new
                {
                    email = request.Email,
                    password = request.Password,
                    tipo = request.TipoUsuario ?? ""
                };

                _logger.LogInformation($"Enviando a UsuariosSRV4: {JsonSerializer.Serialize(requestData)}");

                var response = await _http.PostAsJsonAsync("api/Usuarios/validar-credenciales", requestData);

                var statusCode = response.StatusCode;
                _logger.LogInformation($"Status Code: {statusCode}");

                if (statusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning("Credenciales inválidas");
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Credenciales inválidas"
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Error en UsuariosSRV4: {statusCode} - {errorContent}");
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = $"Error del servidor: {statusCode}"
                    };
                }

                var userResponse = await response.Content.ReadFromJsonAsync<ValidarCredencialesResponse>();

                if (userResponse == null)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    };
                }

                _logger.LogInformation($"Usuario encontrado: ID={userResponse.Id}, Email={userResponse.Email}, Tipo={userResponse.TipoUsuario}");

                if (!userResponse.Activo)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Usuario inactivo"
                    };
                }

                if (userResponse.Bloqueado)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Usuario bloqueado por intentos fallidos"
                    };
                }

                var user = new UserInfoDto
                {
                    Id = userResponse.Id,
                    Email = userResponse.Email,
                    NombreCompleto = userResponse.NombreCompleto,
                    TipoUsuario = userResponse.TipoUsuario,
                    Activo = userResponse.Activo,
                    TipoUsuarioId = 1
                };

                var accessToken = GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken();

                // ✅ Guardar refresh token (SOLO columnas que existen)
                var refreshTokenEntity = new RefreshToken
                {
                    Token = refreshToken,
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false
                };

                _db.RefreshTokens.Add(refreshTokenEntity);
                await _db.SaveChangesAsync();

                return new LoginResponseDto
                {
                    Success = true,
                    Message = "Login exitoso",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenType = "Bearer",
                    ExpiresIn = 3600,
                    User = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en LoginAsync: {ex.Message}");
                return new LoginResponseDto
                {
                    Success = false,
                    Message = $"Error al autenticar: {ex.Message}"
                };
            }
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var storedToken = await _db.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

                if (storedToken == null)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Refresh token inválido"
                    };
                }

                if (storedToken.ExpiresAt < DateTime.UtcNow)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Refresh token expirado"
                    };
                }

                var response = await _http.GetAsync($"api/Usuarios/{storedToken.UserId}");

                if (!response.IsSuccessStatusCode)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Usuario no encontrado"
                    };
                }

                var user = await response.Content.ReadFromJsonAsync<UserInfoDto>();

                if (user == null || !user.Activo)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Usuario inactivo"
                    };
                }

                storedToken.IsRevoked = true;
                await _db.SaveChangesAsync();

                var newAccessToken = GenerateAccessToken(user);
                var newRefreshToken = GenerateRefreshToken();

                var newRefreshTokenEntity = new RefreshToken
                {
                    Token = newRefreshToken,
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false
                };

                _db.RefreshTokens.Add(newRefreshTokenEntity);
                await _db.SaveChangesAsync();

                return new LoginResponseDto
                {
                    Success = true,
                    Message = "Token renovado exitosamente",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    TokenType = "Bearer",
                    ExpiresIn = 3600,
                    User = user
                };
            }
            catch (Exception ex)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = $"Error al renovar token: {ex.Message}"
                };
            }
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            try
            {
                var storedToken = await _db.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

                if (storedToken != null)
                {
                    storedToken.IsRevoked = true;
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? "YourSuperSecretKeyHere12345678901234567890");

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"] ?? "CUC",
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"] ?? "CUCApp",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal != null;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateAccessToken(UserInfoDto user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? "YourSuperSecretKeyHere12345678901234567890");
            var issuer = _configuration["Jwt:Issuer"] ?? "CUC";
            var audience = _configuration["Jwt:Audience"] ?? "CUCApp";

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.NombreCompleto),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.TipoUsuario ?? "Usuario"),
                new Claim("TipoUsuarioId", user.TipoUsuarioId?.ToString() ?? "")
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[64];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}