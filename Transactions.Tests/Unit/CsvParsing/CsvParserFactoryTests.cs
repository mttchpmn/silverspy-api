using System;
using System.Threading.Tasks;
using Transactions.Domain;
using Transactions.Domain.CsvParsing.Parsers;
using Xunit;

namespace Transactions.Tests.CsvParsing;

public class CsvParserFactoryTests
{
    private readonly CsvParserFactory _parserFactory;

    public CsvParserFactoryTests()
    {
        _parserFactory = new CsvParserFactory();
    }

    [Fact]
    public void ArgumentOutOfRangeException_is_thrown_for_unsupported_bank()
    {
        var bankType = "NotSupportedBank";

        Assert.Throws<ArgumentOutOfRangeException>(() => _parserFactory.GetParser(bankType));
    }

    [Fact]
    public void AsbCsvParser_is_returned_for_asb_bankType()
    {
        var bankType = "ASB";

        var parser = _parserFactory.GetParser(bankType);

        Assert.IsType<AsbCsvParser>(parser);
    }
}