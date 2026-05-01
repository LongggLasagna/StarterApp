namespace StarterApp.Tests.Fixtures;

/// <summary>
/// Provides shared database configuration for integration-style tests.
/// The connection string matches the PostgreSQL/PostGIS service used in GitHub Actions.
/// </summary>
public class DatabaseFixture
{
    public string ConnectionString =>
        "Host=localhost;Port=5432;Database=test_db;Username=test_user;Password=test_password";
}