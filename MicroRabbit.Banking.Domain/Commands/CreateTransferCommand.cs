using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Domain.Commands;

public class CreateTransferCommand : TransferCommand
{
    public CreateTransferCommand(Guid from, Guid to, decimal amount) : base(from, to, amount)
    {
    }
}
