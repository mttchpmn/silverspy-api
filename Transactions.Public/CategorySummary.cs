namespace Transactions.Public;

public record CategorySummary(
    TransactionCategory Category,
    decimal CurrentSpend,
    decimal Budget
);

public record CategorySummaryDto(
    string Category,
    decimal CurrentSpend,
    decimal? Budget
    );