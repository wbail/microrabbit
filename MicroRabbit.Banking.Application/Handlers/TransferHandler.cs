using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Domain.Core.Bus;
using Microsoft.Extensions.Logging;

namespace MicroRabbit.Banking.Application.Handlers;

public class TransferHandler : IRequestHandler<AccountTransferRequest>
{
    private readonly IEventBus _eventBus;
    private readonly IAccountTransferValidator _accountTransferValidator;
    private readonly ILogger<TransferHandler> _logger;

    public TransferHandler(
        IEventBus eventBus,
        IAccountTransferValidator accountTransferValidator,
        ILogger<TransferHandler> logger)
    {
        _eventBus = eventBus;
        _accountTransferValidator = accountTransferValidator;
        _logger = logger;
    }

    public async Task Handle(AccountTransferRequest request, CancellationToken cancellationToken)
    {
        var isAccountTransferValid = await _accountTransferValidator.IsAccountTransferRequestValid(request);

        if (!isAccountTransferValid.IsValid)
        {
            foreach (var error in isAccountTransferValid.Errors)
            {
                _logger.LogError("Invalid transfer request: {Error}", error);
            }

            return;
        }

        var createTransferCommand = new CreateTransferCommand
        (
            request.AccountFrom,
            request.AccountTo,
            request.TransferAmount
        );

        await _eventBus.SendCommand(createTransferCommand);
        
        _logger.LogInformation("Transfered from account '{AccountFrom}' to account '{AccountTo}' the amount '{TransferAmount}'", request.AccountFrom, request.AccountTo, request.TransferAmount);
    }
}
