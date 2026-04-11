using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Repositories;
using TaskManagement.Infrastructure.Identity;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Infrastructure.Extensions;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

        services.AddScoped<ITodoItemRepository, TodoItemRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}

