using LoginSRV1.DTOs;
using LoginSRV1.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginSRV1.Endpoints
{
    public static class LoginEndpoints
    {
        public static void MapLoginEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/auth").WithTags("Auth");

            // POST: /api/auth/login
            group.MapPost("/login", async (
                [FromBody] LoginRequestDto request,
                IAuthService authService,
                HttpContext httpContext) =>
            {
                // Obtener IP y User Agent
                request.IpAddress = httpContext.Connection.RemoteIpAddress?.ToString();
                request.UserAgent = httpContext.Request.Headers["User-Agent"].ToString();

                var result = await authService.LoginAsync(request);
                return Results.Ok(result);
            })
            .WithName("Login")
            .Produces<LoginResponseDto>();

            // POST: /api/auth/refresh
            group.MapPost("/refresh", async (
                [FromBody] RefreshTokenRequestDto request,
                IAuthService authService) =>
            {
                var result = await authService.RefreshTokenAsync(request.RefreshToken);
                return Results.Ok(result);
            })
            .WithName("RefreshToken")
            .Produces<LoginResponseDto>();

            // POST: /api/auth/logout
            group.MapPost("/logout", async (
                [FromBody] LogoutRequestDto request,
                IAuthService authService) =>
            {
                var result = await authService.LogoutAsync(request.RefreshToken);
                return Results.Ok(new { success = result });
            })
            .WithName("Logout")
            .Produces<bool>();

            // GET: /api/auth/validate
            group.MapGet("/validate", async (
                [FromQuery] string token,
                IAuthService authService) =>
            {
                var isValid = await authService.ValidateTokenAsync(token);
                return Results.Ok(new { valid = isValid });
            })
            .WithName("ValidateToken")
            .Produces<bool>();
        }
    }
}