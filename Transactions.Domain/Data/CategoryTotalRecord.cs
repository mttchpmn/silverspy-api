using Transactions.Public;

namespace Transactions.Domain.Data;

public record CategoryTotalRecord
{
    public CategoryTotalRecord(string category,
        decimal sum)
    {
        this.category = category;
        this.sum = sum;
    }

    public string category { get; init; }
    public decimal sum { get; init; }

    public CategoryTotal ToCategoryTotal() => new (string.IsNullOrWhiteSpace(category) ? "Uncategorized" : category, sum);
}