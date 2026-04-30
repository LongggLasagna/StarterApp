public class DatabaseFixtureTests
{
    [Fact]
    public void Fixture_ReturnsValidConnection()
    {
        // Arrange
        var fixture = new DatabaseFixture();

        // Act
        var conn = fixture.ConnectionString;

        // Assert
        Assert.Contains("test_db", conn);
    }
}

