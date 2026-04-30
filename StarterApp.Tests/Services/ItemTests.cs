using StarterApp.Database.Models;
using Xunit;

namespace StarterApp.Tests.Services;

public class ItemTests
{
    [Fact]
    public void Item_CanStoreValuesCorrectly()
    {
        var item = new Item
        {
            Title = "Bike",
            Description = "Good bike",
            DailyRate = 15
        };

        Assert.Equal("Bike", item.Title);
        Assert.Equal("Good bike", item.Description);
        Assert.Equal(15, item.DailyRate);
    }
}
