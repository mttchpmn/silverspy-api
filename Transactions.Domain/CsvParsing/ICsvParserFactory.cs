using Transactions.Domain.CsvParsing;
using Transactions.Domain.CsvParsing.Parsers;

namespace Transactions.Domain;

public interface ICsvParserFactory
{
    ICsvParser GetParser(string bankType);
}

public class CsvParserFactory : ICsvParserFactory
{
    public ICsvParser GetParser(string bankType)
    {
        return bankType switch
        {
            "ASB" => new AsbCsvParser(),
            _ => throw new ArgumentOutOfRangeException(nameof(bankType), "Bank type not supported")
        };
    }
}