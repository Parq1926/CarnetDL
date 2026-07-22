using LoginSRV1.DTOs;
using LoginSRV1.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginSRV1.Endpoints
{
    public static class LoginEndpoints
    {
        public static void MapLoginEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/auth");

            group.MapPost("/login", LoginAsync);
            group.MapPost("/refresh", RefreshTokenAsync);
            group.MapPost("/logout", LogoutAsync);
            group.MapGet("/validate", ValidateTokenAsync);
        }

        private static async Task<IResult> LoginAsync(
            [FromBody] LoginRequestDto request,
            IAuthService authService)
        {
            var result = await authService.LoginAsync(request);
            return Results.Ok(result);
        }

        private static async Task<IResult> RefreshTokenAsync(
            [FromBody] RefreshTokenRequestDto request,
            IAuthService authService)
        {
            var result = await authService.RefreshTokenAsync(request.RefreshToken);
            return Results.Ok(result);
        }

        private static async Task<IResult> LogoutAsync(
            [FromBody] RefreshTokenRequestDto request,
            IAuthService authService)
        {
            var result = await authService.LogoutAsync(request.RefreshToken);
            return Results.Ok(new { success = result });
        }

        private static async Task<IResult> ValidateTokenAsync(
            [FromQuery] string token,
            IAuthService authService)
        {
            var result = await authService.ValidateTokenAsync(token);
            return Results.Ok(new { valid = result });
        }
    }
}