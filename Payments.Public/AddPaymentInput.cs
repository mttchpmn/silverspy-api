namespace Payments.Public;

public record ApiAddPaymentInput(
    string ReferenceDate,
    string Type,
    string Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value,
    string EndDate
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
    decimal Value,
    string EndDate
);

public static class PaymentInputExtensions
{
    public static AddPaymentInput ToAddPaymentInput(this ApiAddPaymentInput input)
    {
        var date = DateTime.Parse(input.ReferenceDate);
        var endDate = !string.IsNullOrWhiteSpace(input.EndDate) ? DateTime.Parse(input.EndDate) : (DateTime?) null;
        var type = Enum.Parse<PaymentType>(input.Type, true);
        var freq = Enum.Parse<PaymentFrequency>(input.Frequency, true);

        return new AddPaymentInput(date, type, freq, input.Name, input.Category, input.Details, input.Value, endDate);
    }

    public static UpdatePaymentInput ToUpdatePaymentInput(this ApiUpdatePaymentInput input)
    {
        var date = DateTime.Parse(input.ReferenceDate);
        var endDate = !string.IsNullOrWhiteSpace(input.EndDate) ? DateTime.Parse(input.EndDate) : (DateTime?) null;
        var type = Enum.Parse<PaymentType>(input.Type, true);
        var freq = Enum.Parse<PaymentFrequency>(input.Frequency, true);

        return new UpdatePaymentInput(input.Id, date, type, freq, input.Name, input.Category, input.Details,
            input.Value, endDate);
    }
}

public record AddPaymentInput(
    DateTime ReferenceDate,
    PaymentType Type,
    PaymentFrequency Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value,
    DateTime? EndDate
);

public record UpdatePaymentInput(
    int PaymentId,
    DateTime ReferenceDate,
    PaymentType Type,
    PaymentFrequency Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value,
    DateTime? EndDate) : AddPaymentInput(ReferenceDate,
    Type,
    Frequency,
    Name,
    Category,
    Details,
    Value, 
    EndDate);