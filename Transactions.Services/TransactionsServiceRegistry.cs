using Microsoft.Extensions.DependencyInjection;
using Transactions.Domain;
using Transactions.Domain.Data;
using Transactions.Public;

namespace Transactions.Services;

public static class TransactionsServiceRegistry
{
   public static void RegisterServices(IServiceCollection serviceCollection)
   {
      serviceCollection.AddScoped<ITransactionsService, TransactionsService>();
      serviceCollection.AddScoped<ITransactionsRepository, TransactionsRepository>();
      serviceCollection.AddScoped<ICsvParserFactory, CsvParserFactory>();
   }
}