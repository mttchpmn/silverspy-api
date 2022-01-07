using Transactions.Domain.CsvParsers;

namespace Transactions.Domain;

public interface ICsvParserFactory
{
    ICsvParser GetParser(string bankType);
}

public class CsvParserFactory : ICsvParserFactory
{
    public ICsvParser GetParser(string bankType)
    {
        switch (bankType)
        {
            case "ASB":
                return new AsbCsvParser();
            default:
                throw new ArgumentOutOfRangeException(nameof(bankType), "Bank type not supported");
        }
    }
}