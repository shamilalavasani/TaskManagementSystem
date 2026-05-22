namespace TaskManagement.API.Extensions;

public static class JwtConfigurationExtensions
{
    private const int MinimumKeyLength = 32;

    public static void ValidateJwtSettings(this IConfiguration configuration)
    {
        var key = configuration["JwtSettings:Key"];
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(
                "JwtSettings:Key is missing. Set it with User Secrets (Development) or environment variable JwtSettings__Key (Production).");
        }

        if (key.Length < MinimumKeyLength)
        {
            throw new InvalidOperationException(
                $"JwtSettings:Key must be at least {MinimumKeyLength} characters.");
        }

        if (string.IsNullOrWhiteSpace(configuration["JwtSettings:Issuer"]))
            throw new InvalidOperationException("JwtSettings:Issuer is missing.");

        if (string.IsNullOrWhiteSpace(configuration["JwtSettings:Audience"]))
            throw new InvalidOperationException("JwtSettings:Audience is missing.");
    }
}
