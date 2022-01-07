using Transactions.Public;

namespace Transactions.Domain;

public class TransactionsService : ITransactionsService
{
    public Task<IEnumerable<Transaction>> ImportTransactions(string transactionsCsvString)
    {
        throw new NotImplementedException();
    }
}