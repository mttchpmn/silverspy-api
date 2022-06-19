using Payments.Public;

namespace Payments.Domain;

public interface IPaymentsRepository
{
    Task<Payment> AddPayment(string authId, AddPaymentInput input);
    Task<Payment> UpdatePayment(string authId, UpdatePaymentInput input);
    Task DeletePayment(string authId, int paymentId);
    Task<IEnumerable<Payment>> GetAllPayments(string authId);
    Task<Payment?> GetPaymentById(string authId, int paymentId);
}