namespace TaskManagement.API.Extensions;

public record UserContext(string UserId, bool IsAdminOrManager)
{
    public void Deconstruct(out string userId, out bool isAdminOrManager)
    {
        userId = UserId;
        isAdminOrManager = IsAdminOrManager;
    }
}