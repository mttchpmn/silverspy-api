namespace Payments.Public;

public record GetPaymentForecastInput(
    DateTime StartDate,
    DateTime EndDate
);