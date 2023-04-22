namespace Transactions.Public;

public record TransactionResponse(IEnumerable<TransactionDto> Transactions, IEnumerable<CategoryTotalDto> CategoryTotals, decimal TotalIncoming, decimal TotalOutgoing, decimal NetPosition);

public record TransactionDto(int Id, string TransactionId, DateTime TransactionDate, string Reference, string Description,
    decimal Value, string Type, string Category, string Details)
{
    public static TransactionDto FromTransaction(Transaction t) => new (t.Id, t.TransactionId,
        t.TransactionDate, t.Reference, t.Description, t.Value, t.Type.ToString().ToUpper(),
        t.Category.ToString().ToUpper(), t.Details ?? "");
}
