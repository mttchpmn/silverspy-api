using Transactions.Public;

namespace Transactions.Domain.CsvParsers;

public class AsbCsvParser : ICsvParser
{
    public Task<IEnumerable<Transaction>> Parse(string csvData)
    {
        throw new NotImplementedException();
    }
}