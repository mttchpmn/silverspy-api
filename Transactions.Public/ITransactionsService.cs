namespace Transactions.Public;

public interface ITransactionsService
{
   Task<IEnumerable<Transaction>> ImportTransactions(ImportTransactionsInput input);
}