using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
      serviceCollection.AddScoped<IAkahuService, AkahuService>();

      RegisterDbConnectionFactory(serviceCollection);
   }

   private static void RegisterDbConnectionFactory(IServiceCollection serviceCollection)
   {
      var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? throw new InvalidOperationException("Env variable 'DATABASE_CONNECTION_STRING' is unset");
      
      var connectionFactory = new DatabaseConnectionFactory(connectionString);
      
      serviceCollection.AddSingleton(connectionFactory);
   }
}