using StarterApp.Database.Models;
using Xunit;

namespace StarterApp.Tests.Services;

public class ReviewTests
{
    [Fact]
    public void Review_StoresRatingCorrectly()
    {
        var review = new Review
        {
            Rating = 5,
            Comment = "Great!"
        };

        Assert.Equal(5, review.Rating);
        Assert.Equal("Great!", review.Comment);
    }
}
