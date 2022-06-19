using Test.Utilities;
using Xunit;

namespace Payments.Tests.Integration;

[CollectionDefinition("Test database")]
public class DatabaseCollection : ICollectionFixture<TestDatabaseFixture>
{
}