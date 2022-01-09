namespace Transactions.Public;

public record UpdateTransactionInput(
    int TransactionId,
    string Category,
    string Details);