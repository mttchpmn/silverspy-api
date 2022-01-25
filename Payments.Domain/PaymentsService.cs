using Payments.Public;

namespace Payments.Domain;

public class PaymentsService : IPaymentsService
{
    private readonly IPaymentsRepository _paymentsRepository;

    public PaymentsService(IPaymentsRepository paymentsRepository)
    {
        _paymentsRepository = paymentsRepository;
    }
    
    public async Task<Payment> AddPayment(string authId, AddPaymentInput input)
    {
        return await _paymentsRepository.AddPayment(authId, input);
    }
}