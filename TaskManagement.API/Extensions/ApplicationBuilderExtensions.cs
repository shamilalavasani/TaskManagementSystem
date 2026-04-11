using Microsoft.AspNetCore.Identity;
using TaskManagement.Infrastructure.Identity;

namespace TaskManagement.API.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task SeedRolesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await RoleSeeder.SeedRolesAsync(roleManager);
    }
}
