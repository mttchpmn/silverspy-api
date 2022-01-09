namespace Transactions.Public;

public record ImportTransactionsInput(
    string BankType,
    string CsvData
    );
    