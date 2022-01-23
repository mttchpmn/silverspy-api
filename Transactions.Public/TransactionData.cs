namespace Transactions.Public;

public record TransactionData(IEnumerable<Transaction> Transactions, IEnumerable<CategoryTotal> CategoryTotals, decimal TotalIncoming, decimal TotalOutgoing, decimal NetPosition);