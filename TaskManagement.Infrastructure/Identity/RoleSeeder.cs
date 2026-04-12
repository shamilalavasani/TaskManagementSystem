using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.Common.Security;

namespace TaskManagement.Infrastructure.Identity;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        [
            AppRoles.Admin,
            AppRoles.Manager,
            AppRoles.User
        ];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create role {role}: {errors}");
                }
            }
        }
    }
}