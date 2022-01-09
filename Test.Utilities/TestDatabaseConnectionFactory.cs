using Utilities;

namespace Test.Utilities;

public class TestDatabaseConnectionFactory : DatabaseConnectionFactory
{
    private TestDatabaseConnectionFactory(string connectionString) : base(connectionString)
    {
    }

    public static TestDatabaseConnectionFactory GetFactory()
    {
        var connectionString = "Server=localhost;Port=5432;Database=silverspy_test;User ID=postgres;Password=postgres";

        return new TestDatabaseConnectionFactory(connectionString);
    }
}