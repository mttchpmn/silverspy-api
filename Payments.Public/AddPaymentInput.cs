namespace Payments.Public;

public record AddPaymentInput(
    DateTime ReferenceDate,
    PaymentType Type,
    PaymentFrequency Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value
);

public record UpdatePaymentInput(int PaymentId,
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