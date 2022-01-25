namespace Payments.Public;

public interface IPaymentsService
{
    Task<Payment> AddPayment(string authId, AddPaymentInput input);
    Task<PaymentsSummary> GetPaymentsSummary(string authId, GetPaymentsSummaryInput input);
}