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

    public async Task<TransactionData> GetTransactionData(string authId)
    {
        // TODO - Unit tests
        var transactions = await _transactionsRepository.GetTransactions(authId);
        var categoryTotals = await _transactionsRepository.GetCategoryTotals(authId);
        var totals = await _transactionsRepository.GetTransactionTotals(authId);

        return new TransactionData(transactions, categoryTotals, totals.TotalIncoming, totals.TotalOutgoing, totals.NetPosition);
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