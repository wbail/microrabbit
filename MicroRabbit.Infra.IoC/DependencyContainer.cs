using MediatR;
using MediatR.NotificationPublishers;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroRabbit.Infra.IoC;

public static class DependencyContainer
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // MediatR
        _ = services.AddMediatR(c =>
        {
            c.Lifetime = ServiceLifetime.Singleton;
            c.NotificationPublisher = new TaskWhenAllPublisher();
            _ = c.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(RabbitMQBus))!);
        });

        // Domain Bus
        _ = services.AddTransient<IEventBus, RabbitMQBus>();

        // Domain banking commands
        services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

        // Domain Events

        services.AddTransient<IEventHandler<Transfer.Domain.Events.TransferCreatedEvent>, TransferEventHandler>();

        // Data
        services.AddDbContext<BankingDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("BankingDbContext"));
        });

        services.AddDbContext<TransferDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("TransferDbContext"));
        });

        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<ITransferRepository, TransferRepository>();

        // Application Services
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransferService, TransferService>();

        
    }
}
