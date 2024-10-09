using FluentValidation;
using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Application.Validators;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroRabbit.Infra.IoC;

public static class DependencyContainer
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddControllers();

        // Domain Bus
        _ = services.AddTransient<IEventBus, RabbitMQBus>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new RabbitMQBus(sp.GetRequiredService<IMediator>(), sp.GetRequiredService<IOptions<RabbitMqProperties>>(), scopeFactory);
        });

        // Subscriptions
        _ = services.AddTransient<TransferEventHandler>();

        // Domain banking commands
        _ = services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

        // Domain Events

        _ = services.AddTransient<IEventHandler<Transfer.Domain.Events.TransferCreatedEvent>, TransferEventHandler>();

        // Data
        _ = services.AddDbContext<BankingDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("BankingDbContext"));
        });

        _ = services.AddDbContext<TransferDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("TransferDbContext"));
        });

        _ = services.AddTransient<IAccountRepository, AccountRepository>();
        _ = services.AddTransient<ITransferRepository, TransferRepository>();

        // Application Services
        _ = services.AddScoped<IAccountService, AccountService>();
        _ = services.AddScoped<ITransferService, TransferService>();
        _ = services.AddTransient<IAccountTransferValidator, AccountTransferValidator>();
        _ = services.AddScoped<IValidator<AccountTransferRequest>, AccountTransferValidator>();

        _ = services.AddSerilog();

        _ = services.Configure<RabbitMqProperties>(configuration.GetSection("RabbitMQ"));
    }
}
