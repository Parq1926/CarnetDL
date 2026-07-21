using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginSRV1.Entities;
using Microsoft.IdentityModel.Tokens;

namespace LoginSRV1.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Usuario usuario)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "MiClavePredeterminada1234567890!!");
            var issuer = _configuration["JwtSettings:Issuer"] ?? "CUC";
            var audience = _configuration["JwtSettings:Audience"] ?? "CUCAapp";

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario?.Nombre ?? "Usuario")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "MiClavePredeterminada1234567890!!");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"] ?? "CUC",
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"] ?? "CUCAapp",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}