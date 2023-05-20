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
        var allPayments = (await _paymentsRepository.GetAllPayments(authId)).ToList();
        var apiPayments = allPayments.Select(x => x.ToApiPayment()).ToList();

        // TODO - Handle calculations correctly. Normalise to actual monthly costs
        var monthlyIn = allPayments.Where(x => x.Type.Equals(PaymentType.Incoming)).ToList();
        var monthlyOut = allPayments.Where(x => x.Type.Equals(PaymentType.Outgoing)).ToList();

        var monthlyInTotal = monthlyIn.Sum(x => x.GetMonthlyCost());
        var monthlyOutTotal = monthlyOut.Sum(x => x.GetMonthlyCost());


        var monthlyInSummary = new Summary(monthlyIn.Count, monthlyInTotal);
        var monthlyOutSummary = new Summary(monthlyOut.Count, monthlyOutTotal);
        var monthlyNetSummary = new Summary(monthlyIn.Count + monthlyOut.Count, monthlyInTotal - monthlyOutTotal);

        var categories = apiPayments.Select(x => x.Category).Distinct().ToList();

        var categoryTotals = categories
            .Select(x => new CategoryTotal(x,
                allPayments.Where(y => y.Category.ToString().ToUpper().Equals(x)).Sum(y => y.GetMonthlyCost())))
            .ToList();

        return new PaymentsResponse(apiPayments, monthlyInSummary, monthlyOutSummary, monthlyNetSummary,
            categoryTotals);
    }

    public async Task<PaymentForecast> GetPaymentForecast(string authId, GetPaymentForecastInput input)
    {
        var payments = await _paymentsRepository.GetAllPayments(authId);

        var paymentInstances = payments.SelectMany(x => x.ToPaymentInstances(input.StartDate, input.EndDate)).ToList();
        paymentInstances.Sort((a, b) => a.PaymentDate.CompareTo(b.PaymentDate));

        var totalOutgoing = paymentInstances.Where(x => x.Type == PaymentType.Outgoing).Sum(x => x.Value);
        var totalIncoming = paymentInstances.Where(x => x.Type == PaymentType.Incoming).Sum(x => x.Value);

        var netPosition = totalIncoming - totalOutgoing;

        return new PaymentForecast(paymentInstances.Select(x => x.ToApiPaymentInstance()), totalIncoming, totalOutgoing, netPosition);
    }

    public async Task<PaymentsPeriod> GetPaymentsPeriod(string authId, GetPaymentForecastInput input)
    {
        var payments = await _paymentsRepository.GetAllPayments(authId);

        var result = new List<PaymentWithDate>();

        foreach (var payment in payments)
        {
            var dates = payment.GenerateDates(input.StartDate, input.EndDate).ToList();

            var mapped = dates.Select(x => new PaymentWithDate(payment, x)).ToList();

            result.AddRange(mapped);
        }

        result.Sort((x, y) => DateTime.Compare(x.PaymentDate, y.PaymentDate));

        return new PaymentsPeriod(result);
    }
}