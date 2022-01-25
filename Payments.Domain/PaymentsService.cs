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

    public async Task<IEnumerable<PaymentWithDates>> GetPaymentsSummary(string authId, GetPaymentsSummaryInput input)
    {
        var payments = await _paymentsRepository.GetAllPayments(authId);

        var result = payments.Select(x => x.ToPaymentWIthDates(input.StartDate, input.EndDate));

        return result;
    }
}