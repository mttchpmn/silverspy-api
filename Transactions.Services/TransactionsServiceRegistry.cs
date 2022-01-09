using Microsoft.Extensions.DependencyInjection;
using Transactions.Domain;
using Transactions.Domain.Data;
using Transactions.Public;
using Utilities;

namespace Transactions.Services;

public static class TransactionsServiceRegistry
{
   public static void RegisterServices(IServiceCollection serviceCollection)
   {
      serviceCollection.AddScoped<ITransactionsService, TransactionsService>();
      serviceCollection.AddScoped<ITransactionsRepository, TransactionsRepository>();
      serviceCollection.AddScoped<ICsvParserFactory, CsvParserFactory>();

      RegisterDbConnectionFactory(serviceCollection);
   }

   private static void RegisterDbConnectionFactory(IServiceCollection serviceCollection)
   {
      // TODO - Use env variable
      var connectionString = "Server=localhost;Port=5432;Database=silverspy;User ID=postgres;Password=postgres";
      var connectionFactory = new DatabaseConnectionFactory(connectionString);
      
      serviceCollection.AddSingleton(connectionFactory);
   }
}