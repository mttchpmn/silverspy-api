using Transactions.Public;

namespace Transactions.Domain;

public class TransactionsService : ITransactionsService
{
    private readonly ICsvParserFactory _csvParserFactory;

    public TransactionsService(ICsvParserFactory csvParserFactory)
    {
        _csvParserFactory = csvParserFactory;
    }
    public Task<IEnumerable<Transaction>> ImportTransactions(ImportTransactionsInput input)
    {
        var parser = _csvParserFactory.GetParser(input.BankType);
        var transactions = parser.Parse(input.CsvData);

        return transactions;
    }
}