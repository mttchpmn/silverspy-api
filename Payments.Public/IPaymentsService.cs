namespace Payments.Public;

public interface IPaymentsService
{
    Task<Payment> AddPayment(string authId, AddPaymentInput input);
    Task<Payment?> GetPaymentById(string authId, int paymentId);
    Task<Payment> UpdatePayment(string authId, UpdatePaymentInput input);
    Task DeletePayment(string authId, int paymentId);
    Task<PaymentsSummary> GetPaymentsSummary(string authId, GetPaymentsSummaryInput input);
}