using StarterApp.Database.Models;
using Xunit;

namespace StarterApp.Tests.Services;

public class RentalStatusTests
{
    [Fact]
    public void Rental_AllStatuses_HaveCorrectHelperFlags()
    {
        var rental = new Rental { Status = RentalStatus.Requested };
        Assert.True(rental.IsRequested);
        Assert.False(rental.IsApproved);
        Assert.False(rental.IsOutForRent);
        Assert.False(rental.IsReturned);
        Assert.False(rental.IsCompleted);

        rental.Status = RentalStatus.Approved;
        Assert.False(rental.IsRequested);
        Assert.True(rental.IsApproved);
        Assert.False(rental.IsOutForRent);
        Assert.False(rental.IsReturned);
        Assert.False(rental.IsCompleted);

        rental.Status = RentalStatus.OutForRent;
        Assert.False(rental.IsRequested);
        Assert.False(rental.IsApproved);
        Assert.True(rental.IsOutForRent);
        Assert.False(rental.IsReturned);
        Assert.False(rental.IsCompleted);

        rental.Status = RentalStatus.Returned;
        Assert.False(rental.IsRequested);
        Assert.False(rental.IsApproved);
        Assert.False(rental.IsOutForRent);
        Assert.True(rental.IsReturned);
        Assert.False(rental.IsCompleted);

        rental.Status = RentalStatus.Completed;
        Assert.False(rental.IsRequested);
        Assert.False(rental.IsApproved);
        Assert.False(rental.IsOutForRent);
        Assert.False(rental.IsReturned);
        Assert.True(rental.IsCompleted);
    }

    [Fact]
    public void Rental_StoresCoreFields()
    {
        var item = new Item { Id = 10, Title = "Drill" };
        var borrower = new User { Id = 20, Email = "borrower@test.com" };

        var rental = new Rental
        {
            Id = 1,
            ItemId = item.Id,
            Item = item,
            BorrowerId = borrower.Id,
            Borrower = borrower,
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2026, 1, 3),
            TotalPrice = 25,
            Status = RentalStatus.Approved,
            CreatedAt = new DateTime(2026, 1, 1)
        };

        Assert.Equal(1, rental.Id);
        Assert.Equal(10, rental.ItemId);
        Assert.Equal(item, rental.Item);
        Assert.Equal(20, rental.BorrowerId);
        Assert.Equal(borrower, rental.Borrower);
        Assert.Equal(new DateTime(2026, 1, 1), rental.StartDate);
        Assert.Equal(new DateTime(2026, 1, 3), rental.EndDate);
        Assert.Equal(25, rental.TotalPrice);
        Assert.Equal(RentalStatus.Approved, rental.Status);
        Assert.Equal(new DateTime(2026, 1, 1), rental.CreatedAt);
    }
}