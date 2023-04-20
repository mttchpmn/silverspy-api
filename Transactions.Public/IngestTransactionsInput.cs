using Transactions.Domain;

namespace Transactions.Public;

public record IngestTransactionsInput(string AkahuId, string AkahuToken);