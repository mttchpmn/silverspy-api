using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Transactions.Public;

namespace Transactions.Domain.CsvParsing.Parsers;

public class AsbCsvParser : BaseCsvParser, ICsvParser
{
    public Task<IEnumerable<Transaction>> Parse(string csvData)
    {
        var records = GetRecords<AsbTransaction>(csvData);
        var transactions = records.Select(ToTransaction).ToList();

        return Task.FromResult((IEnumerable<Transaction>)transactions);
    }

    protected override bool ShouldSkipRecord(ShouldSkipRecordArgs shouldSkipRecordArgs)
    {
        var firstCell = shouldSkipRecordArgs.Record[0];

        return firstCell.StartsWith("Created")
               || firstCell.StartsWith("Card")
               || firstCell.StartsWith("From")
               || firstCell.StartsWith("To");
    }

    private Transaction ToTransaction(AsbTransaction asbTransaction)
    {
        return new Transaction(
            0, // TODO - This should come from DB. Or just make nullable?
            asbTransaction.TransactionId,
            asbTransaction.TransactionDate,
            asbTransaction.ProcessedDate,
            asbTransaction.Reference,
            asbTransaction.Description,
            asbTransaction.Amount,
            asbTransaction.Type,
            "",
            ""
        );
    }
}

public record AsbTransaction
{
    [Name("Date Processed")]
    public DateTime ProcessedDate { get; set; }
    [Name("Date of Transaction")]
    public DateTime TransactionDate { get; set; }
    [Name("Unique Id")]
    public int TransactionId { get; set; }
    [Name("Tran Type")]
    public TransactionType Type { get; set; }
    [Name("Reference")]
    public string Reference { get; set; }
    [Name("Description")]
    public string Description { get; set; }
    [Name("Amount")]
    public decimal Amount { get; set; }
}