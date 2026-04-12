namespace TaskManagement.Application.Common.Security;

public static class AppPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string ManagerOrAdmin = "ManagerOrAdmin";
    public const string UserOrAbove = "UserOrAbove";

    public const string CanManageCategories = "CanManageCategories";
    public const string CanDeleteCategories = "CanDeleteCategories";

    public const string CanViewAllTodos = "CanViewAllTodos";
    public const string CanManageAllTodos = "CanManageAllTodos";
}