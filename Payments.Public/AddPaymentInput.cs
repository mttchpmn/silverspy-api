namespace Payments.Public;

public record AddPaymentInput(
    string ReferenceDate,
    string Type,
    string Frequency,
    string Name,
    string Category,
    string Details,
    decimal Value
);