using Test.Utilities;
using Xunit;

namespace Transactions.Tests.Integration;

[CollectionDefinition("Test database")]
public class DatabaseCollection : ICollectionFixture<TestDatabaseFixture>
{
}