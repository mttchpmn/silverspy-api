using Microsoft.Extensions.DependencyInjection;
using Transactions.Domain;
using Transactions.Public;

namespace Transactions.Services;

public static class TransactionsServiceRegistry
{
   public static void RegisterServices(IServiceCollection serviceCollection)
   {
      serviceCollection.AddScoped<ITransactionsService, TransactionsService>();
   }
}