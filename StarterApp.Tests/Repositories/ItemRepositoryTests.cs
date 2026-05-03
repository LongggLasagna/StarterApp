using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Tests.Fixtures;
using Xunit;

namespace StarterApp.Tests.Repositories;

public class ItemRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public ItemRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetDatabase();
    }

    [Fact]
    public async Task GetAllAsync_WhenItemsExist_ReturnsItemsOrderedByTitle()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new ItemRepository(context);

        // Act
        var items = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(items);
        Assert.Equal("Camera", items[0].Title);
        Assert.Equal("Drill", items[1].Title);
    }

    [Fact]
    public async Task GetByIdAsync_WhenItemExists_ReturnsCorrectItem()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new ItemRepository(context);

        // Act
        var item = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(item);
        Assert.Equal("Camera", item.Title);
        Assert.Equal("Electronics", item.Category);
    }

    [Fact]
    public async Task AddAsync_WhenItemIsValid_AddsItemToDatabase()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new ItemRepository(context);

        var item = new Item
        {
            Title = "Tent",
            Description = "Two-person tent",
            DailyRate = 12,
            CategoryId = 3,
            Category = "Camping",
            Location = "Edinburgh",
            Latitude = 55.95,
            Longitude = -3.18,
            OwnerId = 1,
            IsAvailable = true
        };

        // Act
        await repository.AddAsync(item);
        var items = await repository.GetAllAsync();

        // Assert
        Assert.Contains(items, i => i.Title == "Tent");
    }

    [Fact]
    public async Task UpdateAsync_WhenItemChanges_SavesUpdatedValues()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new ItemRepository(context);

        var item = await repository.GetByIdAsync(1);
        Assert.NotNull(item);

        item.Title = "Updated Camera";

        // Act
        await repository.UpdateAsync(item);
        var updated = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Updated Camera", updated.Title);
    }
}