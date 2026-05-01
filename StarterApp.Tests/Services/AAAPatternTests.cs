using StarterApp.Database.Models;
using Xunit;

namespace StarterApp.Tests.Services;

public class AAAPatternEvidenceTests
{
    [Fact]
    public void Item_StoresTitleAndDailyRate_UsingAAAPattern()
    {
        // Arrange
        var item = new Item
        {
            Title = "Camera",
            DailyRate = 15
        };

        // Act
        var title = item.Title;
        var dailyRate = item.DailyRate;

        // Assert
        Assert.Equal("Camera", title);
        Assert.Equal(15, dailyRate);
    }

    [Fact]
    public void Rental_WithCompletedStatus_ReturnsCompletedFlag_UsingAAAPattern()
    {
        // Arrange
        var rental = new Rental
        {
            Status = RentalStatus.Completed
        };

        // Act
        var isCompleted = rental.IsCompleted;

        // Assert
        Assert.True(isCompleted);
        Assert.False(rental.IsRequested);
        Assert.False(rental.IsApproved);
    }

    [Fact]
    public void Review_StoresRatingAndComment_UsingAAAPattern()
    {
        // Arrange
        var review = new Review
        {
            Rating = 5,
            Comment = "Great rental experience"
        };

        // Act
        var rating = review.Rating;
        var comment = review.Comment;

        // Assert
        Assert.Equal(5, rating);
        Assert.Equal("Great rental experience", comment);
    }
}