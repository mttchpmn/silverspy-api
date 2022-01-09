﻿using Transactions.Domain.Data;
using Transactions.Public;

namespace Transactions.Domain;

public class TransactionsService : ITransactionsService
{
    private readonly ICsvParserFactory _csvParserFactory;
    private readonly ITransactionsRepository _transactionsRepository;

    public TransactionsService(ICsvParserFactory csvParserFactory, ITransactionsRepository transactionsRepository)
    {
        _transactionsRepository = transactionsRepository;
        _csvParserFactory = csvParserFactory;
    }
    public async Task<IEnumerable<Transaction>> ImportTransactions(string authId, ImportTransactionsInput input)
    {
        var parser = _csvParserFactory.GetParser(input.BankType);
        var rawTransactions = await parser.Parse(input.CsvData);

        var importedTransactions = await _transactionsRepository.ImportTransactions(authId, rawTransactions);

        return importedTransactions;
    }

    public Task<IEnumerable<Transaction>> GetTransactions(string authId)
    {
        return _transactionsRepository.GetTransactions(authId);
    }
}