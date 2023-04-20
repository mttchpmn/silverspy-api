using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Transactions.Public;

namespace Transactions.Domain;

public class AkahuService : IAkahuService
{
    private readonly ILogger<AkahuService> _logger;

    public AkahuService(ILogger<AkahuService> logger)
    {
        _logger = logger;
    }
    
    public async Task<IEnumerable<RawTransaction>> GetTransactions(string akahuId, string akahuToken)
    {
        
        string? cursor = null;
        var transactions = new List<AkahuTransaction>();

        do
        {
            var response = await GetTransactions(cursor, akahuId, akahuToken);
            transactions.AddRange(response.Transactions);
            cursor = response.Cursor.Next;
        } while (cursor != null);
        
        _logger.LogInformation("Fetched {TransactionCount} transactions from Akahu", transactions.Count);

        var rawTransactions = ConvertTransactions(transactions);

        return rawTransactions;
    }

    private static IEnumerable<RawTransaction> ConvertTransactions(IEnumerable<AkahuTransaction> akahuTransactions)
    {
        return akahuTransactions.Select(x => new RawTransaction(x.Id, DateTime.Parse(x.Date), null,
            x.MerchantDetails?.Name ?? x.Description, x.Description, x.Amount, GetTranType(x.Type), x.CategoryDetails?.Groups?.PersonalFinance?.Name ?? ""));
    }

    private static TransactionType GetTranType(string argType)
    {
        return argType switch
        {
            "DEBIT" => TransactionType.DEBIT,
            "CREDIT" => TransactionType.CREDIT,
            "TRANSFER" => TransactionType.TFR_OUT,
            "EFTPOS" => TransactionType.EFTPOS,
            "PAYMENT" => TransactionType.DEBIT,
            "CREDIT CARD" => TransactionType.DEBIT,
            "DIRECT DEBIT" => TransactionType.DEBIT,
            "DIRECT CREDIT" => TransactionType.CREDIT,
            "FEE" => TransactionType.DEBIT,
            "LOAN" => TransactionType.DEBIT,
            _ => throw new Exception($"Unknown Transaction Type: {argType}")
        };
    }

    private static async Task<AkahuResponse> GetTransactions(string? cursor, string akahuId, string authToken)
    {
        var baseUrl = "https://api.akahu.io/v1/transactions";
        var apiUrl = cursor != null ? $"{baseUrl}?cursor={cursor}" : baseUrl;

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Akahu-ID", akahuId);
        httpClient.DefaultRequestHeaders.Add("Authorization", authToken);

        var response = await httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode) throw new Exception("Error getting transactions from Akahu");

        var responseContent = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};

        var akahuResponse = JsonSerializer.Deserialize<AkahuResponse>(responseContent, options);

        if (akahuResponse == null)
            throw new Exception("Unable to deserialize Akahu response");

        return akahuResponse;
    }
}

public record AkahuResponse
{
    public bool Success { get; init; }
    [JsonPropertyName("items")] public List<AkahuTransaction> Transactions { get; init; }
    public Cursor Cursor { get; init; }
}

public record Cursor(string? Next);

public record AkahuTransaction
{
    [JsonPropertyName("_id")] public string Id { get; init; }
    public string Date { get; init; }
    public string Description { get; init; }
    public decimal Amount { get; init; }
    public string Type { get; init; }
    [JsonPropertyName("merchant")] 
    public MerchantDetails MerchantDetails { get; init; }
    [JsonPropertyName("category")]
    public CategoryDetails CategoryDetails { get; init; }
}

public record MerchantDetails(string Name);

public record CategoryDetails
{
    public string Name { get; init; }
    public GroupsDetail Groups { get; init; }
};

public record GroupsDetail
{
    [JsonPropertyName("personal_finance")]
    public PersonalFinanceGroup PersonalFinance { get; init; }
}

public record PersonalFinanceGroup
{
    public string Name { get; init; }
}