﻿using Dapper;
using Npgsql;
using Transactions.Public;
using Utilities;

namespace Transactions.Domain.Data;

public class TransactionsRepository : ITransactionsRepository
{
    private readonly DatabaseConnectionFactory _databaseConnectionFactory;

    public TransactionsRepository()
    {
        _databaseConnectionFactory = new DatabaseConnectionFactory("Server=localhost;Port=5432;Database=silverspy;User ID=postgres;Password=postgres");
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
            // await using var connection = await _databaseConnectionFactory.GetConnection();
            //
            // var sql =
            //     @"SELECT id, transaction_id, transaction_date, processed_date, reference, description, type, value FROM transaction WHERE id = ANY(@TransactionIds)";
            //
            // var records = (await connection.QueryAsync<TransactionRecord>(sql, transactionIds.ToList())).ToList();
            //
            // return records.Select(x => x.ToTransaction()).ToList();

            return new List<Transaction>();
    }
}