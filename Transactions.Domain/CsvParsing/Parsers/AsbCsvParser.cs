using CsvHelper;
using Transactions.Public;

namespace Transactions.Domain.CsvParsing.Parsers;

public class AsbCsvParser : BaseCsvParser, ICsvParser
{
    public Task<IEnumerable<Transaction>> Parse(string csvData)
    {
        throw new NotImplementedException();
    }

    protected override bool ShouldSkipRecord(ShouldSkipRecordArgs shouldSkipRecordArgs)
    {
        throw new NotImplementedException();
    }
}