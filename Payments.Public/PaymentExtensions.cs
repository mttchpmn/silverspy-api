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