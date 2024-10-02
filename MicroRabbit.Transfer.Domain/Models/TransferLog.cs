namespace MicroRabbit.Transfer.Domain.Models;

public class TransferLog
{
    public Guid Id { get; set; }
    public Guid FromAccount { get; set; }
    public Guid ToAccount { get; set; }
    public decimal TransferAmount { get; set; }
}
