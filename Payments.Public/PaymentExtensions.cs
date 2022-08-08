using System.Globalization;

namespace Payments.Public;

public static class PaymentExtensions
{
    public static IEnumerable<DateTime> GenerateDates(this Payment payment, DateTime startDate, DateTime endDate)
    {
        var result = new List<DateTime>();
        var referenceDate = payment.ReferenceDate;

        while (referenceDate < endDate)
        {
            result.Add(referenceDate);

            referenceDate = payment.Frequency switch
            {
                PaymentFrequency.Weekly => referenceDate.AddDays(7),
                PaymentFrequency.Fortnightly => referenceDate.AddDays(14),
                PaymentFrequency.Monthly => referenceDate.AddMonths(1),
                PaymentFrequency.Yearly => referenceDate.AddYears(1),
                _ => referenceDate
            };
        }

        return result.Where(x => x >= startDate);
    }

    public static decimal GetMonthlyCost(this Payment payment)
    {
        return payment.Frequency switch
        {
            PaymentFrequency.Weekly => payment.Value * 4.3M,
            PaymentFrequency.Fortnightly => payment.Value * 2M,
            PaymentFrequency.Monthly => payment.Value,
            PaymentFrequency.Yearly => payment.Value / 12M,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static PaymentWithDates ToPaymentWIthDates(this Payment payment, DateTime startDate, DateTime endDate)
    {
        var dates = payment.GenerateDates(startDate, endDate);

        return new PaymentWithDates(payment, dates);
    }

    public static ApiPayment ToApiPayment(this Payment payment)
        => new(
            payment.Id,
            payment.Name, payment.ReferenceDate.ToString(CultureInfo.InvariantCulture), payment.Type.ToString(),
            payment.Frequency.ToString(),
            payment.Category, payment.Details, payment.Value);
}