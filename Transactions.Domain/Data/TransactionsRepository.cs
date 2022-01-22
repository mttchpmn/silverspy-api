using Dapper;
using Npgsql;
using Transactions.Public;
using Utilities;

namespace Transactions.Domain.Data;

public class TransactionsRepository : ITransactionsRepository
{
    private readonly DatabaseConnectionFactory _databaseConnectionFactory;

    public TransactionsRepository(DatabaseConnectionFactory databaseConnectionFactory)
    {
        _databaseConnectionFactory = databaseConnectionFactory;
        // new DatabaseConnectionFactory(
        //     "Server=localhost;Port=5432;Database=silverspy;User ID=postgres;Password=postgres");
    }

    public async Task<IEnumerable<Transaction>> ImportTransactions(string authId,
        IEnumerable<RawTransaction> rawTransactions)
    {
        var importedTransactionIds = new List<int>();

        foreach (var rawTransaction in rawTransactions)
        {
            var id = await ImportTransaction(authId, rawTransaction);

            if (id != null)
                importedTransactionIds.Add(id.Value);
        }

        var importedTransactions = await GetTransactionsForIds(importedTransactionIds);

        return importedTransactions;
    }

    public async Task<IEnumerable<Transaction>> GetTransactions(string authId)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql =
            @"SELECT id, auth_id, transaction_id, transaction_date, processed_date, reference, description, value, type, category, details
                    FROM transaction
                    WHERE auth_id = @AuthId";

        var records = await connection.QueryAsync<TransactionRecord>(sql, new {AuthId = authId});

        var result = records.Select(x => x.ToTransaction()).ToList();

        return result;
    }

    public async Task<IEnumerable<CategoryTotal>> GetCategoryTotals(string authId)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql = "SELECT category, SUM(value) FROM transaction WHERE auth_id = @AuthId GROUP BY category";

        var records = (await connection.QueryAsync<CategoryTotalRecord>(sql, new { AuthId = authId })).ToList();

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

        var records = (await connection.QueryAsync<TotalRecord>(sql, new { AuthId = authId })).ToList();

        var incoming = records.Single(x => x.type.Equals(TransactionType.CREDIT));
        var outgoing = records.Single(x => x.type.Equals(TransactionType.DEBIT));
        var netPosition = incoming.sum - outgoing.sum;

        return new TransactionTotals(incoming.sum, outgoing.sum, netPosition);
    }

    public record TotalRecord(TransactionType type, decimal sum);

    private async Task<int?> ImportTransaction(string authid, RawTransaction transaction)
    {
        try
        {
            await using var connection = await _databaseConnectionFactory.GetConnection();

            var sql =
                @"INSERT INTO transaction (
                         auth_id,
                         transaction_id, 
                         transaction_date, 
                         processed_date, 
                         reference, 
                         description, 
                         value, 
                         type) 
                VALUES (
                        @AuthId,
                        @TransactionId,
                        @TransactionDate,
                        @ProcessedDate,
                        @Reference,
                        @Description,
                        @Value,
                        @Type)
                RETURNING id";

            var transactionId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                AuthId = authid,
                transaction.TransactionId,
                transaction.TransactionDate,
                transaction.ProcessedDate,
                transaction.Reference,
                transaction.Description,
                transaction.Value,
                transaction.Type,
            });

            return transactionId;
        }
        catch (PostgresException e)
        {
            if (e.IsDuplicateException())
            {
                Console.WriteLine($"Duplicate: {transaction.TransactionId}");
                return null;
            }

            throw new Exception(e.Message, e);
        }
    }

    private async Task<IEnumerable<Transaction>> GetTransactionsForIds(IEnumerable<int> transactionIds)
    {
        await using var connection = await _databaseConnectionFactory.GetConnection();

        var sql = @"SELECT id, transaction_id, transaction_date, processed_date, reference, description, type, value, category, details 
                    FROM transaction 
                    WHERE id = ANY(@TransactionIds)";

        var records =
            (await connection.QueryAsync<TransactionRecord>(sql, new {TransactionIds = transactionIds.ToList()}))
            .ToList();

        return records.Select(x => x.ToTransaction()).ToList();
    }
}