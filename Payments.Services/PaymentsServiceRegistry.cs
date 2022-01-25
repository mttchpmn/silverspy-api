using Microsoft.Extensions.DependencyInjection;
using Payments.Domain;
using Payments.Public;
using Utilities;

namespace Payments.Services;

public static class PaymentsServiceRegistry
{
   public static void RegisterServices(IServiceCollection serviceCollection)
   {
      serviceCollection.AddScoped<IPaymentsService, PaymentsService>();
      serviceCollection.AddScoped<IPaymentsRepository, PaymentsRepository>();
      
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