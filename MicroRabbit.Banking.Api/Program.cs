using MicroRabbit.Infra.IoC;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

OtelConfiguration.AddOpenTelemetryWithSerilog(builder.Logging, builder.Environment, builder.Configuration);
OtelConfiguration.AddOpenTelemetryWithSerilog(builder.Services, builder.Configuration);
HttpLoggingConfiguration.AddHttpLogging(builder.Services);
SwaggerConfiguration.AddSwagger(builder.Services, builder.Configuration);
MediatRConfiguration.AddMeditR(builder.Services);
DependencyContainer.RegisterServices(builder.Services, builder.Configuration);
HealthCheckConfiguration.AddHealthCheck(builder.Services, builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwaggerConfig();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks();

app.UseSerilogRequestLogging();

await app.RunAsync();

public partial class Program { }
