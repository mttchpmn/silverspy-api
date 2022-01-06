using System;

namespace Database
{
    public class Program
    {
        static int Main(string[] args)
        {
            // TODO - Move connection string to environment variable
            var connectionString = "Server=localhost;Port=5432;Database=silverspy;User ID=postgres;Password=postgres";

            var databaseMigrator = new DatabaseHelper(connectionString);
            var result = databaseMigrator.MigrateDatabase(connectionString);

            if (!result.Successful)
            {
                Console.Error.WriteLine("Error migrating database");
                return -1;
            }

            Console.WriteLine("Database migrated successfully.");
            return 0;
        }
    }
}
