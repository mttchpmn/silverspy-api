namespace Payments.Public;

public record Payment(
    int Id,
    string AuthId,
    DateTime ReferenceDate,
    PaymentType Type,
    PaymentFrequency Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value
);

public record PaymentWithDates : Payment
{
    public PaymentWithDates(Payment payment, IEnumerable<DateTime> PaymentDates) : base(payment.Id,
        payment.AuthId, payment.ReferenceDate, payment.Type, payment.Frequency, payment.Name, payment.Category,
        payment.Details, payment.Value)
    {
        this.PaymentDates = PaymentDates;
    }

    public IEnumerable<DateTime> PaymentDates { get; init; }

    public decimal GetTotalValue()
    {
        return Value * PaymentDates.Count();
    }

}