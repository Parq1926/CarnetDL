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
        private readonly AuthDbContext _authDb;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            AuthDbContext authDb,
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _authDb = authDb;
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                _logger.LogInformation("=== INICIO LOGIN ===");
                _logger.LogInformation($"Email: {request.Email}");

                // 1. Llamar a UsuariosSRV4 para validar credenciales
                var requestData = new
                {
                    email = request.Email,
                    password = request.Password,
                    tipo = request.Tipo ?? ""
                };

                _logger.LogInformation($"Enviando a UsuariosSRV4: {JsonSerializer.Serialize(requestData)}");

                var response = await _httpClient.PostAsJsonAsync("api/Usuarios/validar-credenciales", requestData);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
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
                    _logger.LogWarning($"Error en UsuariosSRV4: {response.StatusCode} - {errorContent}");
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = $"Error del servidor: {response.StatusCode}"
                    };
                }

                // 2. Obtener datos del usuario desde UsuariosSRV4
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

                // 3. Crear objeto UserInfoDto
                var user = new UserInfoDto
                {
                    Id = userResponse.Id,
                    Email = userResponse.Email,
                    NombreCompleto = userResponse.NombreCompleto,
                    TipoUsuario = userResponse.TipoUsuario,
                    Activo = userResponse.Activo,
                    TipoUsuarioId = userResponse.TipoUsuarioId,
                    RolId = userResponse.RolId
                };

                // 4. Generar tokens JWT
                var accessToken = GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken();

                // 5. Guardar refresh token en AuthDB
                var refreshTokenEntity = new RefreshToken
                {
                    UsuarioId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false
                };

                _authDb.RefreshTokens.Add(refreshTokenEntity);
                await _authDb.SaveChangesAsync();

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

        // ✅ Generar Access Token
        private string GenerateAccessToken(UserInfoDto user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? "TuSuperSecretKeyLarga123456789012345678901234567890");
            var issuer = _configuration["Jwt:Issuer"] ?? "CUC";
            var audience = _configuration["Jwt:Audience"] ?? "CUCApp";

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.NombreCompleto),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.TipoUsuario ?? "Usuario"),
                new Claim("TipoUsuarioId", user.TipoUsuarioId?.ToString() ?? ""),
                new Claim("RolId", user.RolId?.ToString() ?? "")
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ✅ Generar Refresh Token
        private string GenerateRefreshToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[64];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        // ✅ Refrescar Token
        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var storedToken = await _authDb.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.IsRevoked == false);

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

                // Obtener usuario de UsuariosSRV4
                var response = await _httpClient.GetAsync($"api/Usuarios/{storedToken.UsuarioId}");

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

                // Revocar token anterior
                storedToken.IsRevoked = true;
                await _authDb.SaveChangesAsync();

                // Generar nuevos tokens
                var newAccessToken = GenerateAccessToken(user);
                var newRefreshToken = GenerateRefreshToken();

                var newRefreshTokenEntity = new RefreshToken
                {
                    UsuarioId = user.Id,
                    Token = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false
                };

                _authDb.RefreshTokens.Add(newRefreshTokenEntity);
                await _authDb.SaveChangesAsync();

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
                _logger.LogError($"Error en RefreshTokenAsync: {ex.Message}");
                return new LoginResponseDto
                {
                    Success = false,
                    Message = $"Error al renovar token: {ex.Message}"
                };
            }
        }

        // ✅ Cerrar sesión
        public async Task<bool> LogoutAsync(string refreshToken)
        {
            try
            {
                var storedToken = await _authDb.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.IsRevoked == false);

                if (storedToken != null)
                {
                    storedToken.IsRevoked = true;
                    await _authDb.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // ✅ Validar token
        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? "TuSuperSecretKeyLarga123456789012345678901234567890");

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
    }
}