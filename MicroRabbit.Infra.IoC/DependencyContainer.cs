using MediatR.NotificationPublishers;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        //_ = services.AddTransient<IEventBus, RabbitMQBus>();

        // Domain Events
        //services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

        // Application Services
        services.AddTransient<IAccountService, AccountService>();
        //services.AddTransient<ITransferService, TransferService>();

        // Data

        services.AddDbContext<BankingDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("BankingDbContext"));
        });

        services.AddTransient<IAccountRepository, AccountRepository>();
        //services.AddTransient<BankingDbContext>();
    }
}
