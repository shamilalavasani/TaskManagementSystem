using System.Text.Json.Serialization;

namespace TaskManagement.API.Extensions;

public static class APIDependencyInjection
{

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}