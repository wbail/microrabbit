using MicroRabbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Domain.Commands;

public abstract class TransferCommand : Command
{
    public int From { get; private set; }
    public int To { get; private set; }
    public decimal Amount { get; private set; }

    public TransferCommand(int from, int to, decimal amount)
    {
        From = from;
        To = to;
        Amount = amount;
    }
}
