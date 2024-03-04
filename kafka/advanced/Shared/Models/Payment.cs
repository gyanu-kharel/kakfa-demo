namespace Shared.Models;

public class Payment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
}