using MediatR;

namespace MicroRabbit.Banking.Application.Models;

public class AccountTransferRequest : IRequest
{
    public Guid AccountFrom { get; set; }
    public Guid AccountTo { get; set; }
    public decimal TransferAmount { get; set; }
}
