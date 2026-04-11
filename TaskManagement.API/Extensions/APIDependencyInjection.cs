using FluentValidation;
using System.Text.Json.Serialization;
using TaskManagement.Application.Validators.CategoryValidators;
using TaskManagement.Application.Validators.TodoItemValidators;
namespace TaskManagement.API.Extensions;

public static class ApiDependencyInjection
{

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());// for runtime api
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.UseInlineDefinitionsForEnums(); // for  swagger test
        });
        services.AddValidatorsFromAssemblyContaining<CreateTodoItemDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
        return services;
    }
}