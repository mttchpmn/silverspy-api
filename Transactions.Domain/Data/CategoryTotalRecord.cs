using Transactions.Public;

namespace Transactions.Domain.Data;

public record CategoryTotalRecord
{
    public CategoryTotalRecord(int category,
        decimal sum)
    {
        this.category = category;
        this.sum = sum;
    }

    public int category { get; init; }
    public decimal sum { get; init; }

    public CategoryTotal ToCategoryTotal() => new ((TransactionCategory)category, sum);
}