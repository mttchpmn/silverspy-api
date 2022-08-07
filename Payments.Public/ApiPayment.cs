namespace Payments.Public;

public record ApiPayment(
    string Name,
    string ReferenceDate,
    string Type,
    string Frequency,
    string Category,
    string Details,
    decimal Value
);