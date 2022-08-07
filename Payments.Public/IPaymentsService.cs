namespace Payments.Public;

public interface IPaymentsService
{
    Task<Payment> AddPayment(string authId, AddPaymentInput input);
    Task<Payment?> GetPaymentById(string authId, int paymentId);
    Task<Payment> UpdatePayment(string authId, UpdatePaymentInput input);
    Task DeletePayment(string authId, int paymentId);
    Task<PaymentsResponse> GetPayments(string authId);
    Task<PaymentsSummary> GetPaymentsSummary(string authId, GetPaymentsSummaryInput input);
    Task<PaymentsPeriod> GetPaymentsPeriod(string authId, GetPaymentsSummaryInput input);
}

public record PaymentsPeriod(IEnumerable<PaymentWithDate> Payments);


public record PaymentsResponse(IEnumerable<ApiPayment> Payments, Summary MonthlyIncoming, Summary MonthlyOutgoing,
    Summary MonthlyNet, IEnumerable<CategoryTotal> CategoryTotals);

public record CategoryTotal(string Category, decimal Total);

public record Summary(int Count, decimal Total);