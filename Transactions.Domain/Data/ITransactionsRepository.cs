using Transactions.Public;

namespace Transactions.Domain.Data;

public interface ITransactionsRepository
{
   Task<IEnumerable<Transaction>> ImportTransactions(IEnumerable<RawTransaction> rawTransactions);
}