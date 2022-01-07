using Transactions.Public;

namespace Transactions.Domain;

public interface ICsvParser
{
    Task<IEnumerable<Transaction>> Parse(string csvData);
}