
using TaskManagement.Application.DTOs.AuthDTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        // Register
        group.MapPost("/register", async (RegisterRequestDto request, IAuthService authService) =>
        {
            var result = await authService.RegisterAsync(request);
            return Results.Ok(result);
        });

        // Login
        group.MapPost("/login", async (LoginRequestDto request, IAuthService authService) =>
        {
            var result = await authService.LoginAsync(request);
            return Results.Ok(result);
        });
    }
}
