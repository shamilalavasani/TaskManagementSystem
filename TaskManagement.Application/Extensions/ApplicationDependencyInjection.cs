using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;




namespace TaskManagement.Application.Extensions;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITodoItemService, TodoItemService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}