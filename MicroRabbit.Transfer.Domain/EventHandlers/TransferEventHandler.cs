﻿using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.EventHandlers;

public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
{
    private readonly ITransferRepository _transferRepository;

    public TransferEventHandler(ITransferRepository transferRepository)
    {
        _transferRepository = transferRepository;
    }

    // TODO: Implement the Handle method
    public Task Handle(TransferCreatedEvent @event)
    {
        Random random = new Random();

        _transferRepository.Add(new TransferLog()
        {
            Id = random.Next(100000000),
            FromAccount = @event.From,
            ToAccount = @event.To,
            TransferAmount = @event.Amount
        });

        return Task.CompletedTask;

    }
}
