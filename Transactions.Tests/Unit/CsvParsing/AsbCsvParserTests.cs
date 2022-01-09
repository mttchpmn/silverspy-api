using System;
using System.Linq;
using System.Threading.Tasks;
using Transactions.Domain.CsvParsing.Parsers;
using Xunit;

namespace Transactions.Tests.CsvParsing;

public class AsbCsvParserTests
{
    private readonly AsbCsvParser _parser;

    public AsbCsvParserTests()
    {
        _parser = new AsbCsvParser();
    }

    [Fact]
    public async Task Valid_input_is_parsed_into_list_of_transactions()
    {
        var input = GetInput();

        var result = (await _parser.Parse(input)).ToList();

        var transactionOne = result[0];
        var transactionTwo = result[1];
        Assert.Equal(2, result.Count());
        
        Assert.Equal("Groceries", transactionOne.Description);
        Assert.Equal(-250.65M, transactionOne.Value);
        
        Assert.Equal("Beers", transactionTwo.Description);
        Assert.Equal(-35.99M, transactionTwo.Value);
    }

    private string GetInput()
    {
        return @"Created date / time : 30 December 2021 / 15:37:34
Card Number 4617-5500-4153-9108 (Visa Platinum Rewards)
From date 20211221
To date 20211230
Date Processed,Date of Transaction,Unique Id,Tran Type,Reference,Description,Amount

2021/12/01,2021/12/01,2021120101,CREDIT,9108,""Groceries"",-250.65
2021/12/01,2021/12/01,2021120101,CREDIT,9108,""Beers"",-35.99
";
    }
}