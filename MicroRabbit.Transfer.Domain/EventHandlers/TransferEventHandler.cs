﻿using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;

namespace MicroRabbit.Transfer.Domain.EventHandlers;

public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
{
    public TransferEventHandler()
    {
        
    }

    // TODO: Implement the Handle method
    public Task Handle(TransferCreatedEvent @event)
    {
        throw new NotImplementedException();
    }
}
