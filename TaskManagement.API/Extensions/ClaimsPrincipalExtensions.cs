using System.Security.Claims;


namespace TaskManagement.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static (string userId, bool isAdminOrManager)? GetUserContext(this ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return null;

        var isAdminOrManager = user.IsInRole("Admin") || user.IsInRole("Manager");
        return (userId, isAdminOrManager);
    }
}