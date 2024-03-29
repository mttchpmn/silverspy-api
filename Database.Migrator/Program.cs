﻿using System;

namespace Database
{
    public class Program
    {
        static int Main(string[] args)
        {
            // var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            var connectionString = "Server=localhost;Port=5432;Database=silverspy;User ID=postgres;Password=postgres";
            if (connectionString == null)
            {
                Console.Error.WriteLine("DB_CONNECTION_STRING env variable not set.");
                return 1;
            }
            Console.WriteLine($"Connecting to DB: {connectionString}");
            
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
