namespace Transactions.Public;

public interface ITransactionsService
{
   Task<IEnumerable<Transaction>> ImportTransactions(string authId, ImportTransactionsInput input);
   Task<IEnumerable<Transaction>> IngestTransactions(string authId, IngestTransactionsInput input);
   Task<IEnumerable<Transaction>> GetTransactions(string authId);
   Task<TransactionDto> UpdateTransaction(string authId, UpdateTransactionInput input);
   
   Task<TransactionResponse> GetTransactionResponse(string authId, DateTime? from, DateTime? dateTime);
}