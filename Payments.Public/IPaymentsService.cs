namespace Payments.Public;

public interface IPaymentsService
{
    Task<Payment> AddPayment(string authId, AddPaymentInput input);
    Task<Payment?> GetPaymentById(string authId, int paymentId);
    Task<Payment> UpdatePayment(string authId, UpdatePaymentInput input);
    Task DeletePayment(string authId, int paymentId);
    Task<PaymentsResponse> GetPayments(string authId);
    Task<PaymentsSummary> GetPaymentsSummary(string authId, GetPaymentsSummaryInput input);
}

public record PaymentsResponse(IEnumerable<Payment> Payments, Summary MonthlyIncomings, Summary MonthlyOutgoings,
    Summary MonthlyNet, IEnumerable<CategoryTotal> CategoryTotals);

public record CategoryTotal(string Category, decimal Total);

public record Summary(int Count, decimal Total);