using Xunit;

namespace StarterApp.Tests.Fixtures;

public class DatabaseFixtureTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public DatabaseFixtureTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void DatabaseFixture_ProvidesCiDatabaseConnectionString()
    {
        // Arrange
        var expectedDatabaseName = "test_db";

        // Act
        var connectionString = _fixture.ConnectionString;

        // Assert
        Assert.Contains(expectedDatabaseName, connectionString);
        Assert.Contains("test_user", connectionString);
        Assert.Contains("test_password", connectionString);
    }
}
