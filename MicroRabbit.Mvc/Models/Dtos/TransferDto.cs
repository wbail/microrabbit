namespace MicroRabbit.Mvc.Models.Dtos;

public class TransferDto
{
    public Guid FromAccount { get; set; }
    public Guid ToAccount { get; set; }
    public decimal TransferAmount { get; set; }
}
