using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace MicroRabbit.Infra.IoC;

public static class OtelConfiguration
{
    public static void AddOpenTelemetryWithLogger(this ILoggingBuilder loggingBuilder, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        loggingBuilder.ClearProviders();

        loggingBuilder.AddOpenTelemetry(x =>
        {
            x.SetResourceBuilder(ResourceBuilder.CreateEmpty()
                .AddService(configuration.GetRequiredSection("ApplicationName").Value!)
                .AddAttributes(new Dictionary<string, object>()
                {
                    ["deployment.environment"] = webHostEnvironment.EnvironmentName
                }));

            x.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(configuration.GetRequiredSection("SeqSettings:ServerUrl").Value!);
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                options.Headers = 
                    $"{configuration.GetRequiredSection("SeqSettings:HeaderName").Value!}={configuration.GetRequiredSection("SeqSettings:SeqApiKey").Value!}";
            });

            x.IncludeScopes = true;
            x.IncludeFormattedMessage = true;
        });
    }

    public static void AddOpenTelemetryWithSerilog(this ILoggingBuilder loggingBuilder, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = configuration.GetRequiredSection("SeqSettings:ServerUrl").Value!;
                options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.HttpProtobuf;
                options.Headers = new Dictionary<string, string>()
                {
                    [configuration.GetRequiredSection("SeqSettings:HeaderName").Value!] 
                        = configuration.GetRequiredSection("SeqSettings:SeqApiKey").Value!
                };
                options.ResourceAttributes = new Dictionary<string, object>()
                {
                    ["service.name"] = configuration.GetRequiredSection("ApplicationName").Value!,
                    ["deployment.environment"] = webHostEnvironment.EnvironmentName
                };
            })
            .CreateLogger();
    }

    public static IServiceCollection AddOpenTelemetryWithSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(configuration.GetRequiredSection("ApplicationName").Value!))
            .WithTracing(tracing =>
            {
                tracing
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();

                tracing.AddOtlpExporter(config =>
                {
                    config.Endpoint = new Uri("http://localhost:30060/ingest/otlp/v1/traces");
                    config.Protocol = OtlpExportProtocol.HttpProtobuf;
                });
            });

        return services;
    }
}
