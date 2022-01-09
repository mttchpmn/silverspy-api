using Npgsql;

namespace Utilities;

public class DatabaseConnectionFactory
{
    private readonly string _connectionString;

    public DatabaseConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Task<NpgsqlConnection> GetConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);

        return Task.FromResult(connection);
    }
}