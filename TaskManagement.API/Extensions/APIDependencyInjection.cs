using FluentValidation;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TaskManagement.Application.Validators.TodoItemValidators;

namespace TaskManagement.API.Extensions;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.UseInlineDefinitionsForEnums();
            // 🔐 تعریف Bearer
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token"
            });

            // 🔐 اعمال به همه endpointها
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
     {


         {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
             },
            new string[] {}
             }
          });
        });

        services.AddValidatorsFromAssemblyContaining<CreateTodoItemDtoValidator>();

        return services;
    }
}