namespace MicroRabbit.Banking.Domain.Models;

public class Account
{
    public Guid Id { get; set; }
    public string AccountType { get; set; } = null!;
    public decimal AccountBalance { get; set; }
}
