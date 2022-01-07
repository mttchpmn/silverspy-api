namespace Transactions.Public;

public record Transaction(
    int Id,
    int TransactionId,
    DateTime Date,
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