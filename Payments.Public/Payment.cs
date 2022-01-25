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