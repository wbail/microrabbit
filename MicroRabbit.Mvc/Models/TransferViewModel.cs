namespace MicroRabbit.Mvc.Models;

public class TransferViewModel
{
    public Guid FromAccount { get; set; }
    public Guid ToAccount { get; set; }
    public decimal TransferAmount { get; set; }
    public string? TransferNotes { get; set; }
}
