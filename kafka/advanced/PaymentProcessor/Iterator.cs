using Bogus;
using Shared.Models;

namespace PaymentProcessor;

public static class Iterator
{
    public static IEnumerable<Payment> GetPayments()
    {
        var payment = new Payment();
        for (int i = 0; i < 50; i++)
        {
            var faker = new Faker();
            payment.Id = Guid.NewGuid();
            payment.UserId = Guid.NewGuid();
            payment.Amount = faker.Finance.Amount();
            payment.TransactionDate = DateTime.UtcNow;

            yield return payment;
        }
    }
}