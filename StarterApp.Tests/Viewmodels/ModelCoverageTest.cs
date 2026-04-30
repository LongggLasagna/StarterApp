using StarterApp.Database.Models;
using Xunit;

namespace StarterApp.Tests.Services;

public class ModelCoverageTests
{
    [Fact]
    public void Item_StoresCoreFields()
    {
        var item = new Item
        {
            Id = 1,
            Title = "Bike",
            Description = "Mountain bike",
            DailyRate = 12,
            Category = "Sports",
            Location = "Edinburgh",
            OwnerId = 5,
            Latitude = 55.9533,
            Longitude = -3.1883,
            IsAvailable = true
        };

        Assert.Equal(1, item.Id);
        Assert.Equal("Bike", item.Title);
        Assert.Equal("Mountain bike", item.Description);
        Assert.Equal(12, item.DailyRate);
        Assert.Equal("Sports", item.Category);
        Assert.Equal("Edinburgh", item.Location);
        Assert.Equal(5, item.OwnerId);
        Assert.True(item.IsAvailable);
    }

    [Fact]
    public void Review_StoresCoreFields()
    {
        var review = new Review
        {
            Id = 1,
            ReviewerId = 2,
            Rating = 5,
            Comment = "Great",
            CreatedAt = new DateTime(2026, 1, 1)
        };

        Assert.Equal(1, review.Id);
        Assert.Equal(2, review.ReviewerId);
        Assert.Equal(5, review.Rating);
        Assert.Equal("Great", review.Comment);
    }
}
