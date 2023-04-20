namespace Transactions.Public;

public interface ITransactionsService
{
   Task<IEnumerable<Transaction>> ImportTransactions(string authId, ImportTransactionsInput input);
   Task<IEnumerable<Transaction>> IngestTransactions(string authId, IngestTransactionsInput input);
   Task<IEnumerable<Transaction>> GetTransactions(string authId);
   Task<TransactionData> GetTransactionData(string authId, DateTime? from, DateTime? dateTime);
   Task<IEnumerable<CategoryTotal>> GetCategoryTotals(string authId);
   Task<Transaction> UpdateTransaction(string authId, UpdateTransactionInput input);
}