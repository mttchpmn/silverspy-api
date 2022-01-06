namespace Transactions.Public;

public record Transaction(int Id,
    DateTime Date,
    DateTime ProcessedDate,
    string Reference,
    string Particulars,
    decimal Value,
    TransactionType Type,
    string Category,
    string Details);

public enum TransactionType
{
    Debit,
    Credit
}