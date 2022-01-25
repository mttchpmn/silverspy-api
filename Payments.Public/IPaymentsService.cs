namespace Payments.Public;

public interface IPaymentsService
{
    Task<Payment> AddPayment(string authId, AddPaymentInput input);
    Task<IEnumerable<PaymentWithDates>> GetPaymentsSummary(string authId, GetPaymentsSummaryInput input);
}