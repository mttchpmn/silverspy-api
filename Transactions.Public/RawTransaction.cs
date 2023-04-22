namespace Transactions.Public;

public record RawTransaction(
    string TransactionId,
    DateTime TransactionDate,
    DateTime? ProcessedDate,
    string Reference,
    string Description,
    decimal Value,
    TransactionType Type,
    TransactionCategory Category
    );