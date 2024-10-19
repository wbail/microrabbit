using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public static class HttpLoggingConfiguration
{
    public static IServiceCollection AddHttpLogging(this IServiceCollection services)
    {
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.All;

            options.CombineLogs = true;
        });

        return services;
    }
}
