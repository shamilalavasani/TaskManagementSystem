using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.DTOs.AuthDTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (request.Password != request.ConfirmPassword)
            throw new ArgumentException("Password and ConfirmPassword do not match.");

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            throw new ArgumentException("A user with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);//identity framework will hash the password and store it securely

        if (!result.Succeeded)
        {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            throw new ArgumentException(errors);
        }

        await _userManager.AddToRoleAsync(user, "User");

        var roles = await _userManager.GetRolesAsync(user);// add role to token claims
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email!, roles);
        var expireAt = _jwtTokenService.GetTokenExpiration();

        return new AuthResponseDto //reate response with token and user info
        {
            AccessToken = token,
            ExpireAt = expireAt,
            Email = user.Email!
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new ArgumentException("Invalid email or password.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            throw new ArgumentException("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email!, roles);
        var expireAt = _jwtTokenService.GetTokenExpiration();

        return new AuthResponseDto
        {
            AccessToken = token,
            ExpireAt = expireAt,
            Email = user.Email!
        };
    }
}
