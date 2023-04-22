using Microsoft.Extensions.Logging;
using Transactions.Domain.Data;
using Transactions.Public;

namespace Transactions.Domain;

public class TransactionsService : ITransactionsService
{
    private readonly ICsvParserFactory _csvParserFactory;
    private readonly ITransactionsRepository _transactionsRepository;
    private readonly IAkahuService _akahuService;
    private readonly ILogger<TransactionsService> _logger;

    public TransactionsService(ICsvParserFactory csvParserFactory, ITransactionsRepository transactionsRepository,
        IAkahuService akahuService, ILogger<TransactionsService> logger)
    {
        _transactionsRepository = transactionsRepository;
        _akahuService = akahuService;
        _logger = logger;
        _csvParserFactory = csvParserFactory;
    }

    public async Task<IEnumerable<Transaction>> ImportTransactions(string authId, ImportTransactionsInput input)
    {
        var parser = _csvParserFactory.GetParser(input.BankType);

        var rawTransactions = (await parser.Parse(input.CsvData)).ToList();
        _logger.LogInformation("Parsed {RawTransactionCount} raw transactions", rawTransactions.Count);

        var importedTransactions = (await _transactionsRepository.ImportTransactions(authId, rawTransactions)).ToList();
        _logger.LogInformation("Imported {TransactionCount} transactions", importedTransactions.Count);

        return importedTransactions;
    }

    public async Task<IEnumerable<Transaction>> IngestTransactions(string authId, IngestTransactionsInput input)
    {
        var rawTransactions = await _akahuService.GetTransactions(input.AkahuId, input.AkahuToken);
        var importedTransactions = (await _transactionsRepository.ImportTransactions(authId, rawTransactions)).ToList();

        _logger.LogInformation("Imported {TransactionCount} new transactions", importedTransactions.Count);

        return importedTransactions;
    }

    public async Task<TransactionResponse> GetTransactionData(string authId, DateTime? from, DateTime? to)
    {
        // TODO - Unit tests
        var transactions = await _transactionsRepository.GetTransactions(authId, from, to);
        var transactionDtos = transactions.Select(TransactionDto.FromTransaction).ToList();
        var categoryTotals = await _transactionsRepository.GetCategoryTotals(authId);
        var categoryTotalDtos = categoryTotals.Select(CategoryTotalDto.FromCategoryTotal).ToList();
        var totals = await _transactionsRepository.GetTransactionTotals(authId);

        return new TransactionResponse(transactionDtos, categoryTotalDtos, totals.TotalIncoming, totals.TotalOutgoing,
            totals.NetPosition);
    }

    public Task<IEnumerable<Transaction>> GetTransactions(string authId)
    {
        return _transactionsRepository.GetTransactions(authId);
    }

    public async Task<IEnumerable<CategoryTotal>> GetCategoryTotals(string authId)
    {
        return await _transactionsRepository.GetCategoryTotals(authId);
    }

    public Task<Transaction> UpdateTransaction(string authId, UpdateTransactionInput input)
    {
        return _transactionsRepository.UpdateTransaction(authId, input);
    }
}

