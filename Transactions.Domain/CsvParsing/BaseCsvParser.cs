using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Transactions.Domain.CsvParsing;

public abstract class BaseCsvParser
{
    private readonly CsvConfiguration _config;

    protected BaseCsvParser()
    {
        _config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            ShouldSkipRecord = ShouldSkipRecord
        };
    }

    public IEnumerable<T>? GetRecords<T>(string csvData)
    {
        using var reader = new StringReader(csvData);
        using var csv = new CsvReader(reader, _config);

        try
        {
            return csv.GetRecords<T>().ToList();
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(HeaderValidationException))
            {
                Console.WriteLine("Encountered header validation exception");
                return null;
            }
            Console.WriteLine(e);
            throw;
        }
    }

    protected abstract bool ShouldSkipRecord(ShouldSkipRecordArgs shouldSkipRecordArgs);
}