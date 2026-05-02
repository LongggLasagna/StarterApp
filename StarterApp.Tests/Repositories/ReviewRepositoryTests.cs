using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Tests.Fixtures;
using Xunit;

namespace StarterApp.Tests.Repositories;

public class ReviewRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public ReviewRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetDatabase();
    }

    [Fact]
    public async Task GetForItemAsync_WhenReviewsExist_ReturnsReviewsForItem()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new ReviewRepository(context);

        // Act
        var reviews = await repository.GetForItemAsync(2);

        // Assert
        Assert.NotEmpty(reviews);
        Assert.All(reviews, review => Assert.Equal(2, review.ItemId));
    }

    [Fact]
    public async Task AddAsync_WhenReviewIsValid_AddsReviewToDatabase()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new ReviewRepository(context);

        var review = new Review
        {
            ItemId = 1,
            ReviewerId = 2,
            Rating = 4,
            Comment = "Good rental",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await repository.AddAsync(review);
        var reviews = await repository.GetForItemAsync(1);

        // Assert
        Assert.Contains(reviews, r => r.Comment == "Good rental");
    }

    [Fact]
    public async Task GetAverageRatingForItemAsync_WhenReviewsExist_ReturnsAverage()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new ReviewRepository(context);

        // Act
        var average = await repository.GetAverageRatingForItemAsync(2);

        // Assert
        Assert.Equal(5, average);
    }

    [Fact]
    public async Task GetAverageRatingForItemAsync_WhenNoReviewsExist_ReturnsZero()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new ReviewRepository(context);

        // Act
        var average = await repository.GetAverageRatingForItemAsync(999);

        // Assert
        Assert.Equal(0, average);
    }
}