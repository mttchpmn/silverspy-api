using Transactions.Public;

namespace Transactions.Domain;

public record RawTransaction(
    string TransactionId,
    DateTime TransactionDate,
    DateTime? ProcessedDate,
    string Reference,
    string Description,
    decimal Value,
    TransactionType Type
    );