namespace Test.Utilities;

public class TestDatabaseFixture : IDisposable
{
    private readonly TestDatabase _testDatabase;

    public TestDatabaseFixture()
    {
        _testDatabase = new TestDatabase("Server=localhost;Port=5432;User ID=postgres;Password=postgres;");
        _testDatabase.Initialise();
    }

    public void Dispose()
    {
        _testDatabase.Destroy();
    }
}