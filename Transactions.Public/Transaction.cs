namespace Transactions.Public;

public record Transaction(
    int Id,
    string TransactionId,
    DateTime TransactionDate,
    DateTime ProcessedDate,
    string Reference,
    string Description,
    decimal Value,
    TransactionType Type,
    string Category,
    string Details);

public enum TransactionType
{
    DEBIT,
    CREDIT
}