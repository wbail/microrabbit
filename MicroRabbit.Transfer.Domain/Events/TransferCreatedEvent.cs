using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Transfer.Domain.Events;

public class TransferCreatedEvent : Event
{
    public Guid From { get; private set; }
    public Guid To { get; private set; }
    public decimal Amount { get; private set; }

    public TransferCreatedEvent(Guid from, Guid to, decimal amount)
    {
        From = from;
        To = to;
        Amount = amount;
    }
}
