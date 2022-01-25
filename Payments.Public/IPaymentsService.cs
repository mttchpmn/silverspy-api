namespace Payments.Public;

public interface IPaymentsService
{
    Task<Payment> AddPayment(AddPaymentInput input);
}