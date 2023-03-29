namespace Payments.Public;

public record ApiAddPaymentInput(
    string ReferenceDate,
    string Type,
    string Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value
);

public record ApiImportPaymentsInput(
    List<ApiAddPaymentInput> Payments
);

public record ApiUpdatePaymentInput(
    int Id,
    string ReferenceDate,
    string Type,
    string Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value
);

public static class PaymentInputExtensions
{
    public static AddPaymentInput ToAddPaymentInput(this ApiAddPaymentInput input)
    {
        var date = DateTime.Parse(input.ReferenceDate);
        var type = Enum.Parse<PaymentType>(input.Type, true);
        var freq = Enum.Parse<PaymentFrequency>(input.Frequency, true);

        return new AddPaymentInput(date, type, freq, input.Name, input.Category, input.Details, input.Value);
    }
    
    public static UpdatePaymentInput ToUpdatePaymentInput(this ApiUpdatePaymentInput input)
    {
        var date = DateTime.Parse(input.ReferenceDate);
        var type = Enum.Parse<PaymentType>(input.Type, true);
        var freq = Enum.Parse<PaymentFrequency>(input.Frequency, true);

        return new UpdatePaymentInput(input.Id, date, type, freq, input.Name, input.Category, input.Details, input.Value);
    }
}

public record AddPaymentInput(
    DateTime ReferenceDate,
    PaymentType Type,
    PaymentFrequency Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value
);

public record UpdatePaymentInput(
    int PaymentId,
    DateTime ReferenceDate,
    PaymentType Type,
    PaymentFrequency Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value) : AddPaymentInput(ReferenceDate,
    Type,
    Frequency,
    Name,
    Category,
    Details,
    Value);