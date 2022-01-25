namespace Payments.Public;

public record PaymentsSummary(
    IEnumerable<PaymentWithDates> Payments,
    decimal TotalOutgoing
);