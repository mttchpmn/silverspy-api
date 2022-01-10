namespace Transactions.Public;

public interface ITransactionsService
{
   Task<IEnumerable<Transaction>> ImportTransactions(string authId, ImportTransactionsInput input);
   Task<IEnumerable<Transaction>> GetTransactions(string authId);
   Task<Transaction> UpdateTransaction(string authId, UpdateTransactionInput input);
}