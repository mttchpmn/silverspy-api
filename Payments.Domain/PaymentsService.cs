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

    public async Task<PaymentsSummary> GetPaymentsSummary(string authId, GetPaymentsSummaryInput input)
    {
        var payments = await _paymentsRepository.GetAllPayments(authId);

        var paymentsWithDates = payments.Select(x => x.ToPaymentWIthDates(input.StartDate, input.EndDate)).ToList();

        var totalOutgoing = paymentsWithDates.Sum(x => x.GetTotalValue());

        return new PaymentsSummary(paymentsWithDates, totalOutgoing);
    }
}