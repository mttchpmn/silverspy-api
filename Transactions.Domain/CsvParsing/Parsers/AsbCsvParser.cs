using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Transactions.Public;

namespace Transactions.Domain.CsvParsing.Parsers;

public class AsbCsvParser : BaseCsvParser, ICsvParser
{
    public Task<IEnumerable<RawTransaction>> Parse(string csvData)
    {
        var records = GetRecords(csvData);
        var transactions = records.Select(x => x.ToRawTransaction()).ToList();

        return Task.FromResult((IEnumerable<RawTransaction>)transactions);
    }

    private IEnumerable<IAsbTransaction> GetRecords(string csvData)
    {
        var creditTransactions =  GetRecords<AsbCreditCardTransaction>(csvData);
        if (creditTransactions != null)
            return creditTransactions;
        
        var accountTransactions =  GetRecords<AsbAccountTransaction>(csvData);
        if (accountTransactions != null)
            return accountTransactions;

        throw new InvalidOperationException("Unable to parse input into ASB transactions");
    }

    protected override bool ShouldSkipRecord(ShouldSkipRecordArgs shouldSkipRecordArgs)
    {
        var firstCell = shouldSkipRecordArgs.Record[0];

        return firstCell.StartsWith("Created")
               || firstCell.StartsWith("Card")
               || firstCell.StartsWith("From")
               || firstCell.StartsWith("To");
    }
}

public interface IAsbTransaction
{
    RawTransaction ToRawTransaction();
}

public record AsbCreditCardTransaction : IAsbTransaction
{
    [Name("Date Processed")]
    public DateTime ProcessedDate { get; set; }
    [Name("Date of Transaction")]
    public DateTime TransactionDate { get; set; }
    [Name("Unique Id")]
    public string TransactionId { get; set; }
    [Name("Tran Type")]
    public TransactionType Type { get; set; }
    [Name("Reference")]
    public string Reference { get; set; }
    [Name("Description")]
    public string Description { get; set; }
    [Name("Amount")]
    public decimal Amount { get; set; }

    public RawTransaction ToRawTransaction()
    {
        return new RawTransaction(
            TransactionId,
            TransactionDate,
            ProcessedDate,
            Reference,
            Description,
            Amount,
            Type,
            TransactionCategory.Uncategorized
        );
    }
}

public record AsbAccountTransaction : IAsbTransaction
{
    [Name("Date")]
    public DateTime TransactionDate { get; set; }
    [Name("Unique Id")]
    public string TransactionId { get; set; }
    [TypeConverter(typeof(AsbTransactionTypeConverter))]
    [Name("Tran Type")]
    public TransactionType Type { get; set; }
    [Name("Payee")]
    public string Payee { get; set; }
    [Name("Memo")]
    public string Description { get; set; }
    [Name("Amount")]
    public decimal Amount { get; set; }

    public RawTransaction ToRawTransaction()
    {
        return new RawTransaction(
            TransactionId,
            TransactionDate,
            null,
            Payee,
            Description,
            Amount,
            Type,
            TransactionCategory.Uncategorized
        );
    }
}

public class AsbTransactionTypeConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text switch
        {
            "CREDIT" => TransactionType.CREDIT,
            "DEBIT" => TransactionType.DEBIT,
            "EFTPOS" => TransactionType.EFTPOS,
            "TFR IN" => TransactionType.TFR_IN,
            "TFR OUT" => TransactionType.TFR_OUT,
            _ => throw new InvalidDataException($"Unable to convert value to ASB transaction type: {text}")
        };
    }
}