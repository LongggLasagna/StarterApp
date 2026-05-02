using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Tests.Fixtures;
using Xunit;

namespace StarterApp.Tests.Repositories;

public class RentalRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public RentalRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetDatabase();
    }

    [Fact]
    public async Task GetOutgoingAsync_WhenBorrowerHasRentals_ReturnsBorrowerRentals()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new RentalRepository(context);

        // Act
        var rentals = await repository.GetOutgoingAsync(2);

        // Assert
        Assert.NotEmpty(rentals);
        Assert.All(rentals, rental => Assert.Equal(2, rental.BorrowerId));
    }

    [Fact]
    public async Task GetIncomingAsync_WhenOwnerHasItems_ReturnsIncomingRentals()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new RentalRepository(context);

        // Act
        var rentals = await repository.GetIncomingAsync(1);

        // Assert
        Assert.NotEmpty(rentals);
        Assert.All(rentals, rental => Assert.Equal(1, rental.Item!.OwnerId));
    }

    [Fact]
    public async Task HasOverLappingRentalAsync_WhenDatesOverlap_ReturnsTrue()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new RentalRepository(context);

        // Act
        var hasOverlap = await repository.HasOverLappingRentalAsync(
            itemId: 1,
            startDate: new DateTime(2026, 1, 2),
            endDate: new DateTime(2026, 1, 4));

        // Assert
        Assert.True(hasOverlap);
    }

    [Fact]
    public async Task HasOverLappingRentalAsync_WhenDatesDoNotOverlap_ReturnsFalse()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new RentalRepository(context);

        // Act
        var hasOverlap = await repository.HasOverLappingRentalAsync(
            itemId: 1,
            startDate: new DateTime(2026, 1, 6),
            endDate: new DateTime(2026, 1, 8));

        // Assert
        Assert.False(hasOverlap);
    }

    [Fact]
    public async Task HasCompletedRentalAsync_WhenCompletedRentalExists_ReturnsTrue()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new RentalRepository(context);

        // Act
        var hasCompletedRental = await repository.HasCompletedRentalAsync(
            itemId: 2,
            borrowerId: 2);

        // Assert
        Assert.True(hasCompletedRental);
    }

    [Fact]
    public async Task UpdateAsync_WhenStatusChanges_SavesNewStatus()
    {
        // Arrange
        using var context = _fixture.CreateContext();
        var repository = new RentalRepository(context);

        var rental = await repository.GetByIdAsync(1);
        Assert.NotNull(rental);

        rental.Status = RentalStatus.Returned;

        // Act
        await repository.UpdateAsync(rental);
        var updated = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal(RentalStatus.Returned, updated.Status);
    }
}