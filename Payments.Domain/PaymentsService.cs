using Payments.Public;

namespace Payments.Domain;

public class PaymentsService : IPaymentsService
{
    public async Task<Payment> AddPayment(AddPaymentInput input)
    {
        return new Payment(0, "lkjlk", DateTime.Now, PaymentType.Incoming, PaymentFrequency.Fortnightly, "Bills", "Foo",
            "", 69);
    }
}