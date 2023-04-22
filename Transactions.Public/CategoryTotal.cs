namespace Transactions.Public;

public record CategoryTotal(
    TransactionCategory Category,
    decimal Value
);

public record CategoryTotalDto(string Category, decimal Value)
{
    public static CategoryTotalDto FromCategoryTotal(CategoryTotal total) =>
        new(total.Category.ToString().ToUpper(), total.Value);
}
