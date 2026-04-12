using TaskManagement.Application.Common.Security;

namespace TaskManagement.API.Extensions;

public static class ApiAuthorizationExtensions
{

    public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AppPolicies.AdminOnly, policy =>
                policy.RequireRole(AppRoles.Admin));

            options.AddPolicy(AppPolicies.ManagerOrAdmin, policy =>
                policy.RequireRole(AppRoles.Manager, AppRoles.Admin));

            options.AddPolicy(AppPolicies.UserOrAbove, policy =>
                policy.RequireRole(AppRoles.User, AppRoles.Manager, AppRoles.Admin));

            options.AddPolicy(AppPolicies.CanManageCategories, policy =>
                policy.RequireRole(AppRoles.Manager, AppRoles.Admin));

            options.AddPolicy(AppPolicies.CanDeleteCategories, policy =>
                policy.RequireRole(AppRoles.Admin));

            options.AddPolicy(AppPolicies.CanViewAllTodos, policy =>
                policy.RequireRole(AppRoles.Manager, AppRoles.Admin));

            options.AddPolicy(AppPolicies.CanManageAllTodos, policy =>
                policy.RequireRole(AppRoles.Manager, AppRoles.Admin));
        });

        return services;
    }
}
