namespace Transactions.Public;

public record Transaction(
    int Id,
    string AuthId,
    string TransactionId,
    DateTime TransactionDate,
    DateTime ProcessedDate,
    string Reference,
    string Description,
    decimal Value,
    TransactionType Type,
    TransactionCategory Category,
    string Details);

public enum TransactionType
{
    DEBIT,
    CREDIT,
    EFTPOS,
    TFR_IN,
    TFR_OUT
}