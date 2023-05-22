using System.Globalization;

namespace Payments.Public;

public static class PaymentExtensions
{
    public static IEnumerable<DateTime> GenerateDates(this Payment payment, DateTime startDate, DateTime endDate)
    {
        var result = new List<DateTime>();
        var referenceDate = payment.ReferenceDate;

        while (!(referenceDate > endDate) && (payment.EndDate == null || referenceDate < payment.EndDate))
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

    public static IEnumerable<PaymentInstance> ToPaymentInstances(this Payment payment, DateTime startDate,
        DateTime endDate)
    {
        var dates = payment.GenerateDates(startDate, endDate);

        var instances = dates.Select(x =>
            new PaymentInstance(x, payment.Type, payment.Name, payment.Category, payment.Details, payment.Value));

        return instances;
    }

    public static ApiPaymentInstance ToApiPaymentInstance(this PaymentInstance paymentInstance)
        => new(paymentInstance.PaymentDate.ToUniversalTime().ToString("O"),
            paymentInstance.Type.ToString().ToUpper(), paymentInstance.Name,
            paymentInstance.Category, paymentInstance.Details, paymentInstance.Value);

    public static PaymentWithDates ToPaymentWIthDates(this Payment payment, DateTime startDate, DateTime endDate)
    {
        var dates = payment.GenerateDates(startDate, endDate);

        return new PaymentWithDates(payment, dates);
    }

    public static ApiPayment ToApiPayment(this Payment payment)
        => new(
            payment.Id,
            payment.Name, payment.ReferenceDate.ToUniversalTime().ToString("O"), payment.Type.ToString(),
            payment.Frequency.ToString(),
            payment.Category, payment.Details, payment.Value);
}