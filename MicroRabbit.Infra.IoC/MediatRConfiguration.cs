using MediatR.NotificationPublishers;
using MicroRabbit.Banking.Application.Handlers;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroRabbit.Infra.IoC;

public static class MediatRConfiguration
{
    public static void AddMeditR(this IServiceCollection services)
    {
        // MediatR
        _ = services.AddMediatR(c =>
        {
            c.Lifetime = ServiceLifetime.Singleton;
            c.NotificationPublisher = new TaskWhenAllPublisher();

            _ = c.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(RabbitMQBus))!);

            _ = c.RegisterServicesFromAssemblyContaining<AccountTransferRequest>();
            _ = c.RegisterServicesFromAssemblyContaining<TransferHandler>();
        });
    }
}
