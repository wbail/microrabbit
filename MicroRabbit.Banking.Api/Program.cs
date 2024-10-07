using MicroRabbit.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

OtelConfiguration.AddOpenTelemetryWithSerilog(builder.Logging, builder.Environment, builder.Configuration);
SwaggerConfiguration.AddSwagger(builder.Services, builder.Configuration);
MediatRConfiguration.AddMeditR(builder.Services);
DependencyContainer.RegisterServices(builder.Services, builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwaggerConfig();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

public partial class Program { }
