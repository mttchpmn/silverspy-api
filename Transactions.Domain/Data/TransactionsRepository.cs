using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Transactions.Public;
using Utilities;

namespace Transactions.Domain.Data;

public class TransactionsRepository : ITransactionsRepository
{
    private readonly DatabaseConnectionFactory _databaseConnectionFactory;
    private readonly ILogger<TransactionsRepository> _logger;

    public TransactionsRepository(DatabaseConnectionFactory databaseConnectionFactory,
        ILogger<TransactionsRepository> logger)
    {
        _databaseConnectionFactory = databaseConnectionFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<Transaction>> ImportTransactions(string authId,
        IEnumerable<RawTransaction> rawTransactions)
    {
        var importedTransactionIds = new List<int>();

        foreach (var rawTransaction in rawTransactions)
        {
            var id = await ImportTransaction(authId, rawTransaction);

            if (id == null)
            {
                // Transaction already exists in DB (user is trying to import duplicate)
                continue;
            }

            importedTransactionIds.Add(id.Value);
        }

        var importedTransactions = await GetTransactionsForIds(importedTransactionIds);

        return importedTransactions;
    }

    public async Task<IEnumerable<Transaction>> GetTransactions(string authId, DateTime? from, DateTime? to)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql =
            @"SELECT id, auth_id, transaction_id, transaction_date, processed_date, reference, description, value, type, category, details
                    FROM transaction
                    WHERE auth_id = @AuthId";

        if (from != null)
            sql += " AND transaction_date > @FromDate";
        
        if (to != null)
            sql += " AND transaction_date < @ToDate";

        var records = await connection.QueryAsync<TransactionRecord>(sql, new {AuthId = authId, FromDate = from, ToDate = to});

        var result = records.Select(x => x.ToTransaction()).OrderBy(x => x.Id).ToList();

        return result;
    }

    public async Task<IEnumerable<CategoryTotal>> GetCategoryTotals(string authId)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql = "SELECT category, SUM(value) FROM transaction WHERE auth_id = @AuthId GROUP BY category";

        var records = (await connection.QueryAsync<CategoryTotalRecord>(sql, new {AuthId = authId})).ToList();

        var result = records.Select(x => x.ToCategoryTotal()).ToList();

        return result;
    }

    public async Task<Transaction> UpdateTransaction(string authId, UpdateTransactionInput input)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var existingTransaction = await connection.QuerySingleOrDefaultAsync<TransactionRecord>(
            "SELECT id FROM transaction WHERE auth_id = @AuthId AND id = @TransactionId", new
            {
                AuthId = authId,
                TransactionId = input.TransactionId
            });

        if (existingTransaction == null)
            throw new Exception("Transaction not found"); // TODO - Handle with custom exception

        var sql =
            "UPDATE transaction SET category = @Category, details = @Details WHERE auth_id = @AuthId AND id = @TransactionId";

        var affectedRows = await connection.ExecuteAsync(sql,
            new
            {
                Category = input.Category, Details = input.Details, TransactionId = input.TransactionId, AuthId = authId
            });
        Console.WriteLine($"Affected rows: {affectedRows}");

        var transaction = (await GetTransactionsForIds(new[] {input.TransactionId})).First();

        return transaction;
    }

    public async Task<TransactionTotals> GetTransactionTotals(string authId)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql = "SELECT type, SUM(value) FROM transaction WHERE auth_id = @AuthId GROUP BY type";

        var records = (await connection.QueryAsync<TotalRecord>(sql, new {AuthId = authId})).ToList();

        var incoming = records.SingleOrDefault(x => x.type.Equals(TransactionType.CREDIT))?.sum ?? 0;
        var outgoing = records.SingleOrDefault(x => x.type.Equals(TransactionType.DEBIT))?.sum ?? 0;
        var netPosition = incoming - outgoing;

        return new TransactionTotals(incoming, outgoing, netPosition);
    }

    public record TotalRecord(TransactionType type, decimal sum);

    private async Task<int?> ImportTransaction(string authid, RawTransaction transaction)
    {
        // var uniqueId = $"{transaction.TransactionId}|{transaction.Reference.Length > 3 ? transaction.Reference.Substring(0, 4) : ''}"; // TODO - Is this a good way of doing it?
        try
        {
            await using var connection = await _databaseConnectionFactory.GetConnection();

            var sql =
                @"INSERT INTO transaction (
                         auth_id,
                         transaction_id, 
                         unique_id,
                         transaction_date, 
                         processed_date, 
                         reference, 
                         description, 
                         category,
                         value, 
                         type) 
                VALUES (
                        @AuthId,
                        @TransactionId,
                        @UniqueId,
                        @TransactionDate,
                        @ProcessedDate,
                        @Reference,
                        @Description,
                        @Category,
                        @Value,
                        @Type)
                RETURNING id";

            var transactionId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                AuthId = authid,
                UniqueId = transaction.TransactionId,
                transaction.TransactionId,
                transaction.TransactionDate,
                transaction.ProcessedDate,
                transaction.Reference,
                transaction.Description,
                transaction.Category,
                transaction.Value,
                transaction.Type,
            });
            
            _logger.LogInformation("Imported transaction with ID: {TransactionId} successfully", transactionId);

            return transactionId;
        }
        catch (PostgresException e)
        {
            if (e.IsDuplicateException())
            {
                _logger.LogInformation("Transaction: {TransactionId} already exists in DB, will treat as duplicate", transaction.TransactionId);
                return null;
            }
            
            _logger.LogError("Received Postgres Exception: {Message}", e.Message);
            
            throw new Exception(e.Message, e);
        }
    }

    private async Task<IEnumerable<Transaction>> GetTransactionsForIds(IEnumerable<int> transactionIds)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql =
            @"SELECT id, transaction_id, transaction_date, processed_date, reference, description, type, value, category, details 
                    FROM transaction 
                    WHERE id = ANY(@TransactionIds)";

        var records =
            (await connection.QueryAsync<TransactionRecord>(sql, new {TransactionIds = transactionIds.ToList()}))
            .ToList();

        return records.Select(x => x.ToTransaction()).ToList();
    }
}