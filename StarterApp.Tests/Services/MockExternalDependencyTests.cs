using Xunit;

namespace StarterApp.Tests.Services;

public class MockExternalDependencyTests
{
    [Fact]
    public async Task MockLocationService_WhenCalled_ReturnsExpectedCoordinates()
    {
        // Arrange
        var locationService = new MockLocationService();

        // Act
        var location = await locationService.GetCurrentLocationAsync();

        // Assert
        Assert.Equal(55.9533, location.Latitude);
        Assert.Equal(-3.1883, location.Longitude);
    }

    [Fact]
    public async Task MockApiService_WhenCalled_ReturnsExpectedItemCount()
    {
        // Arrange
        var apiService = new MockApiService();

        // Act
        var items = await apiService.GetItemsAsync();

        // Assert
        Assert.NotEmpty(items);
        Assert.Equal("Mock Camera", items[0]);
    }

    private class MockLocationService
    {
        public Task<TestLocation> GetCurrentLocationAsync()
        {
            return Task.FromResult(new TestLocation(55.9533, -3.1883));
        }
    }

    private class MockApiService
    {
        public Task<List<string>> GetItemsAsync()
        {
            return Task.FromResult(new List<string> { "Mock Camera", "Mock Drill" });
        }
    }

    private record TestLocation(double Latitude, double Longitude);
}