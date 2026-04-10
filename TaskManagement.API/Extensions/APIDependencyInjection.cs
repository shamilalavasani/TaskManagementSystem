using FluentValidation;
using System.Text.Json.Serialization;
using TaskManagement.Application.Validators;
namespace TaskManagement.API.Extensions;

public static class ApiDependencyInjection
{

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());// baraye runtime api
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.UseInlineDefinitionsForEnums(); // baraye test swagger
        });
        services.AddValidatorsFromAssemblyContaining<CreateTodoItemDtoValidator>();
        return services;
    }
}