using Transactions.Public;

namespace Transactions.Domain;

public interface IAkahuService
{
    Task<IEnumerable<RawTransaction>> GetTransactions(string akahuId, string akahuToken);
}