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
    
    public IEnumerable<T> GetRecords<T>(string csvData)
    {
        using var reader = new StringReader(csvData);
        using var csv = new CsvReader(reader, _config);

        return csv.GetRecords<T>();
    }

    protected abstract bool ShouldSkipRecord(ShouldSkipRecordArgs shouldSkipRecordArgs);
}