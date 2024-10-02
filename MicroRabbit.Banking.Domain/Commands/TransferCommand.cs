using MicroRabbit.Domain.Core.Commands;

namespace MicroRabbit.Banking.Domain.Commands;

public abstract class TransferCommand : Command
{
    public Guid From { get; private set; }
    public Guid To { get; private set; }
    public decimal Amount { get; private set; }

    public TransferCommand(Guid from, Guid to, decimal amount)
    {
        From = from;
        To = to;
        Amount = amount;
    }
}
