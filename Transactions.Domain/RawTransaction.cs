using Transactions.Public;

namespace Transactions.Domain;

public record RawTransaction(
    int TransactionId,
    DateTime TransactionDate,
    DateTime ProcessedDate,
    string Reference,
    string Description,
    decimal Value,
    TransactionType Type
    );