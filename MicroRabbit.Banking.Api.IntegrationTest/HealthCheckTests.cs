using MicroRabbit.Banking.Api.IntegrationTest.Configurations;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MicroRabbit.Banking.Api.IntegrationTest;

public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public HealthCheckTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Liveness_HealthCheck_Returns_Ok()
    {
        var client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/health/liveness");

        _ = response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Readiness_HealthCheck_Returns_Ok()
    {
        var client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/health/readiness");

        _ = response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Database_HealthCheck_Returns_Ok()
    {
        var client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/health/database");

        _ = response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task RabbitMq_HealthCheck_Returns_Ok()
    {
        var client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/health/rabbitmq");

        _ = response.EnsureSuccessStatusCode();
    }
}
