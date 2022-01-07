namespace Transactions.Domain.CsvParsing;

public interface ICsvParser
{
    Task<IEnumerable<RawTransaction>> Parse(string csvData);
}