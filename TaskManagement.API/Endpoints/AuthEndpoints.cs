using TaskManagement.Application.DTOs.AuthDTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", Register)
            .AddEndpointFilter<ValidationFilter<RegisterRequestDto>>();

        group.MapPost("/login", Login)
            .AddEndpointFilter<ValidationFilter<LoginRequestDto>>();
    }

    private static async Task<IResult> Register(RegisterRequestDto request, IAuthService authService)
    {
        var result = await authService.RegisterAsync(request);
        return Results.Ok(result);
    }

    private static async Task<IResult> Login(LoginRequestDto request, IAuthService authService)
    {
        var result = await authService.LoginAsync(request);
        return Results.Ok(result);
    }
}
