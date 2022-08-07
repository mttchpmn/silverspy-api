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

    public async Task<Payment?> GetPaymentById(string authId, int paymentId)
    {
        return await _paymentsRepository.GetPaymentById(authId, paymentId);
    }

    public async Task<Payment> UpdatePayment(string authId, UpdatePaymentInput input)
    {
        return await _paymentsRepository.UpdatePayment(authId, input);
    }

    public async Task DeletePayment(string authId, int paymentId)
    {
        await _paymentsRepository.DeletePayment(authId, paymentId);
    }

    public async Task<PaymentsResponse> GetPayments(string authId)
    {
        var allPayments =  (await _paymentsRepository.GetAllPayments(authId)).ToList();
        
        // TODO - Handle calculations correctly. Normalise to actual monthly costs

        var monthlyIn = new Summary(1, 7950.54M);
        var monthlyOut = new Summary(8, 3568M);
        var monthlyNet = new Summary(9, 4402.54M);

        var categoryTotals = new List<CategoryTotal>
        {
            new CategoryTotal("INCOME", 7950.54M),
            new CategoryTotal("FIXED_COSTS", 3568M),
        };
        
        return new PaymentsResponse(allPayments, monthlyIn, monthlyOut, monthlyNet, categoryTotals);
    }

    public async Task<PaymentsSummary> GetPaymentsSummary(string authId, GetPaymentsSummaryInput input)
    {
        var payments = await _paymentsRepository.GetAllPayments(authId);

        var paymentsWithDates = payments.Select(x => x.ToPaymentWIthDates(input.StartDate, input.EndDate)).ToList();

        var totalOutgoing = paymentsWithDates.Sum(x => x.GetTotalValue());

        return new PaymentsSummary(paymentsWithDates, totalOutgoing);
    }
}