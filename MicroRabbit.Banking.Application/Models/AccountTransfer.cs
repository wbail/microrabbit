namespace MicroRabbit.Banking.Application.Models;

public class AccountTransfer
{
    public Guid AccountFrom { get; set; }
    public Guid AccountTo { get; set; }
    public decimal TransferAmount { get; set; }
}
