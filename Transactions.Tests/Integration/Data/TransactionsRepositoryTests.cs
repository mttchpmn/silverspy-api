using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public TransactionsRepositoryTests()
    {
        _transactionsRepository = new TransactionsRepository(TestDatabaseConnectionFactory.GetFactory());
    }

    public class ImportTransactions : TransactionsRepositoryTests
    {
        [Fact]
        public async Task Can_import_transactions()
        {
            var input = GetInput();

            var authId = "test|1234";

            var importedTransactions = await _transactionsRepository.ImportTransactions(authId, input);
            
            Assert.Equal(2, importedTransactions.Count());
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

    public class GetTransactions : TransactionsRepositoryTests
    {
        [Fact]
        public void Can_get_transactions()
        {
        }
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