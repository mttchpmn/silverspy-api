using Transactions.Public;

namespace Transactions.Domain.Data;

public interface ITransactionsRepository
{
   Task<IEnumerable<Transaction>> ImportTransactions(string authid, IEnumerable<RawTransaction> rawTransactions);
   Task<IEnumerable<Transaction>> GetTransactions(string authid, DateTime? from = null, DateTime? to = null);
   Task<IEnumerable<CategoryTotal>> GetCategoryTotals(string authId, DateTime? from, DateTime? to);
   Task<Transaction> UpdateTransaction(string authId, UpdateTransactionInput input);
   Task<TransactionTotals> GetTransactionTotals(string authId);
}