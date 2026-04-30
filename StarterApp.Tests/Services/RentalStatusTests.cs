using StarterApp.Database.Models;
using Xunit;

namespace StarterApp.Tests.Services;

public class RentalStatusTests
{
    [Fact]
    public void Rental_WithRequestedStatus_HasCorrectHelperFlags()
    {
        var rental = new Rental { Status = RentalStatus.Requested };

        Assert.True(rental.IsRequested);
        Assert.False(rental.IsApproved);
        Assert.False(rental.IsOutForRent);
        Assert.False(rental.IsReturned);
        Assert.False(rental.IsCompleted);
    }

    [Fact]
    public void Rental_StatusTransitions_WorkCorrectly()
    {
        var rental = new Rental();

        rental.Status = RentalStatus.Requested;
        Assert.True(rental.IsRequested);

        rental.Status = RentalStatus.Approved;
        Assert.True(rental.IsApproved);

        rental.Status = RentalStatus.OutForRent;
        Assert.True(rental.IsOutForRent);

        rental.Status = RentalStatus.Returned;
        Assert.True(rental.IsReturned);

        rental.Status = RentalStatus.Completed;
        Assert.True(rental.IsCompleted);
    }
}