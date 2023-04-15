using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Test.Utilities;
using Transactions.Domain;
using Transactions.Domain.Data;
using Transactions.Public;
using Xunit;

namespace Transactions.Tests.Integration.Data;

[Collection("Test database")]
public class TransactionsRepositoryTests
{
    private readonly TransactionsRepository _transactionsRepository;
    private readonly string _authId = "test|1234";

    public TransactionsRepositoryTests()
    {
        _transactionsRepository = new TransactionsRepository(TestDatabaseConnectionFactory.GetFactory(), Mock.Of<ILogger<TransactionsRepository>>());
    }

    public class ImportTransactions : TransactionsRepositoryTests
    {
        [Fact]
        public async Task Can_import_transactions()
        {
            var input = GetInput();

            var importedTransactions = (await _transactionsRepository.ImportTransactions(_authId, input)).ToList();

            var transactionOne = importedTransactions[0];
            var transactionTwo = importedTransactions[1];

            Assert.Equal(2, importedTransactions.Count());

            Assert.Equal("Groceries", transactionOne.Description);
            Assert.Equal(159.50M, transactionOne.Value);
            Assert.Equal("111", transactionOne.TransactionId);

            Assert.Equal("Beers", transactionTwo.Description);
            Assert.Equal(49.50M, transactionTwo.Value);
            Assert.Equal("222", transactionTwo.TransactionId);
        }

        [Fact]
        public async Task Duplicate_transactions_are_not_reimported()
        {
            var input = GetInput();

            await _transactionsRepository.ImportTransactions(_authId, input);

            var importedTransactions = await _transactionsRepository.ImportTransactions(_authId, input);

            Assert.Empty(importedTransactions);
        }
    }

    public class GetTransactions : TransactionsRepositoryTests
    {
        [Fact]
        public async Task Can_get_transactions()
        {
            var input = GetInput();
            await _transactionsRepository.ImportTransactions(_authId, input);

            var transactions = (await _transactionsRepository.GetTransactions(_authId)).ToList();

            var transactionOne = transactions[0];
            var transactionTwo = transactions[1];

            Assert.Equal(2, transactions.Count());

            Assert.Equal("Groceries", transactionOne.Description);
            Assert.Equal(159.50M, transactionOne.Value);
            Assert.Equal("111", transactionOne.TransactionId);

            Assert.Equal("Beers", transactionTwo.Description);
            Assert.Equal(49.50M, transactionTwo.Value);
            Assert.Equal("222", transactionTwo.TransactionId);
        }

        [Fact]
        public async Task Only_transactions_for_user_are_returned()
        {
            var input = GetInput();
            await _transactionsRepository.ImportTransactions(_authId, input);

            var transactions = (await _transactionsRepository.GetTransactions(_authId)).ToList();

            Assert.True(transactions.All(x => x.AuthId.Equals(_authId)));
        }
    }

    public class UpdateTransaction : TransactionsRepositoryTests
    {
        [Fact]
        public async Task CanUpdateTransaction()
        {
            var input = GetInput();
            var importedTransactions = await _transactionsRepository.ImportTransactions(_authId, input);
            var transaction = importedTransactions.First();

            var category = "Food and drink";
            var details = "New World Weekly Shop";
            var updateInput = new UpdateTransactionInput(transaction.Id, category, details);

            var updatedTransaction = await _transactionsRepository.UpdateTransaction(_authId, updateInput);

            Assert.Equal(transaction.Id, updatedTransaction.Id);
            Assert.Equal(category, updatedTransaction.Category);
            Assert.Equal(details, updatedTransaction.Details);
        }
    }

    public class GetCategoryTotals : TransactionsRepositoryTests
    {
        [Fact]
        public async Task Can_get_category_totals()
        {
            var input = new List<RawTransaction>()
            {
                new RawTransaction(
                    111,
                    DateTime.UnixEpoch,
                    DateTime.UnixEpoch,
                    "7605",
                    "Groceries",
                    159.50M,
                    TransactionType.DEBIT),
                new RawTransaction(
                    222,
                    DateTime.UnixEpoch,
                    DateTime.UnixEpoch,
                    "7605",
                    "Beers",
                    49.50M,
                    TransactionType.DEBIT),
                new RawTransaction(
                    333,
                    DateTime.UnixEpoch,
                    DateTime.UnixEpoch,
                    "7605",
                    "Misc",
                    39.50M,
                    TransactionType.DEBIT),
            };
            await _transactionsRepository.ImportTransactions(_authId, input);
            await _transactionsRepository.UpdateTransaction(_authId, new UpdateTransactionInput(1, "Shopping", "Foo"));
            await _transactionsRepository.UpdateTransaction(_authId, new UpdateTransactionInput(2, "Shopping", "Bar"));

            var categoryTotals = (await _transactionsRepository.GetCategoryTotals(_authId)).ToList();


            Assert.Equal(2, categoryTotals.Count);

            var shopping = categoryTotals.First();
            var other = categoryTotals[1];
            
            Assert.Equal(209M, shopping.Value);
            Assert.Equal(39.5M, other.Value);
        }
    }

    public class GetTransactionTotals : TransactionsRepositoryTests
    {
        [Fact]
        public async Task Can_get_category_totals()
        {
            var input = new List<RawTransaction>()
            {
                new RawTransaction(
                    111,
                    DateTime.UnixEpoch,
                    DateTime.UnixEpoch,
                    "7605",
                    "Groceries",
                    159.50M,
                    TransactionType.DEBIT),
                new RawTransaction(
                    222,
                    DateTime.UnixEpoch,
                    DateTime.UnixEpoch,
                    "7605",
                    "Beers",
                    49.50M,
                    TransactionType.DEBIT),
                new RawTransaction(
                    333,
                    DateTime.UnixEpoch,
                    DateTime.UnixEpoch,
                    "7605",
                    "Misc",
                    39.50M,
                    TransactionType.CREDIT),
            };
            await _transactionsRepository.ImportTransactions(_authId, input);

            var totals = await _transactionsRepository.GetTransactionTotals(_authId);
            
            Assert.Equal(209M, totals.TotalOutgoing);
            Assert.Equal(39.5M, totals.TotalIncoming);
            Assert.Equal(39.5M - 209M, totals.NetPosition);
        }
    }

    private static List<RawTransaction> GetInput()
    {
        return new List<RawTransaction>()
        {
            new RawTransaction(
                111,
                DateTime.UnixEpoch,
                DateTime.UnixEpoch,
                "7605",
                "Groceries",
                159.50M,
                TransactionType.DEBIT),

            new RawTransaction(
                222,
                DateTime.UnixEpoch,
                DateTime.UnixEpoch,
                "7605",
                "Beers",
                49.50M,
                TransactionType.DEBIT),
        };
    }
}