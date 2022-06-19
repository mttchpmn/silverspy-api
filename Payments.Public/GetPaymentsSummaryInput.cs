namespace Payments.Public;

public record GetPaymentsSummaryInput(
    DateTime StartDate,
    DateTime EndDate
);