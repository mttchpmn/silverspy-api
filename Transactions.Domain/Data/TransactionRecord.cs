using Transactions.Public;

namespace Transactions.Domain.Data;

public record TransactionRecord
{
    public int id { get; init; }
    public string auth_id { get; init; }
    public string transaction_id { get; init; }
    public DateTime transaction_date { get; init; }
    public DateTime processed_date { get; init; }
    public string reference { get; init; }
    public string description { get; init; }
    public TransactionType type { get; init; }
    public decimal value { get; init; }
    public string details { get; init; }
    public string category { get; init; }

    public Transaction ToTransaction()
    {
        return new Transaction(
            id,
            auth_id,
            transaction_id,
            transaction_date,
            processed_date,
            reference,
            description,
            value,
            type,
            category,
            details);
    }
}