using System;
using System.Reflection;
using System.Text.RegularExpressions;
using DbUp;
using DbUp.Engine;
using Npgsql;
using Utilities;

namespace Database
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }


        public void CreateDatabase(string databaseName)
        {
            try
            {
                ExecuteCommand($"CREATE DATABASE {databaseName}");
            }
            catch (PostgresException e)
            {
                // Swallow exception if database already exists
                if (e.IsDatabaseExistsException())
                    return;

                throw;
            }
        }

        public void DropDatabase(string databaseName)
        {
            ExecuteCommand($@"
                SELECT pg_terminate_backend(pg_stat_activity.pid)
                FROM pg_stat_activity
                WHERE pg_stat_activity.datname = '{databaseName}' 
                AND pid <> pg_backend_pid();

                DROP DATABASE {databaseName}");
        }

        public DatabaseUpgradeResult MigrateDatabase(string connectionString)
        {
            Console.WriteLine("Migrating Database...");

			// TODO - Fix this
            // EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgradeEngine = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgradeEngine.PerformUpgrade();

            return result;
        }

        private void ExecuteCommand(string command)
        {
            var connectionString = GetConnectionStringWithoutDatabase();

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var cmd = new NpgsqlCommand(command, connection);

            cmd.ExecuteNonQuery();
        }

        private string GetConnectionStringWithoutDatabase()
        {
            return Regex.Replace(_connectionString, "Database=./;", "");
        }
    }
}