using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;
using MicroRabbit.Banking.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MicroRabbit.Infra.IoC;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BankingDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("BankingDbConnection")));

        services.AddHealthChecks()
            .AddApplicationStatus("Application")
            .AddDbContextCheck<BankingDbContext>(name: "PostgreSQL", tags: new[] { "dependencies" })
            .AddRabbitMQ(new Uri(configuration.GetConnectionString("RabbitMQ")!), name: "RabbitMq", tags: new[] { "dependencies" });

        return services;
    }

    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        _ = app.MapHealthChecks("/health/liveness", new HealthCheckOptions
        {
            Predicate = hc => hc.Name.Equals("Application", StringComparison.OrdinalIgnoreCase),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        _ = app.MapHealthChecks("/health/readiness", new HealthCheckOptions
        {
            Predicate = hc => hc.Name.Equals("dependencies"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        _ = app.MapHealthChecks("/health/database", new HealthCheckOptions
        {
            Predicate = hc => hc.Name.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        _ = app.MapHealthChecks("/health/rabbitmq", new HealthCheckOptions
        {
            Predicate = hc => hc.Name.Equals("RabbitMQ", StringComparison.OrdinalIgnoreCase),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        return app;
    }
}
