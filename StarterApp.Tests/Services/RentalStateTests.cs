using StarterApp.Database.Models;
using StarterApp.Database.States;
using Xunit;

namespace StarterApp.Tests.Services;

public class RentalStateTests
{
    [Theory]
    [InlineData(RentalStatus.Approved, true)]
    [InlineData(RentalStatus.Rejected, true)]
    [InlineData(RentalStatus.OutForRent, false)]
    [InlineData(RentalStatus.Returned, false)]
    [InlineData(RentalStatus.Completed, false)]
    public void RequestedState_CanTransitionToExpectedStatuses(RentalStatus nextStatus, bool expected)
    {
        // Arrange
        IRentalState state = new RequestedState();

        // Act
        var result = state.CanTransitionTo(nextStatus);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(RentalStatus.OutForRent, true)]
    [InlineData(RentalStatus.Completed, false)]
    [InlineData(RentalStatus.Rejected, false)]
    [InlineData(RentalStatus.Requested, false)]
    public void ApprovedState_CanTransitionToExpectedStatuses(RentalStatus nextStatus, bool expected)
    {
        // Arrange
        IRentalState state = new ApprovedState();

        // Act
        var result = state.CanTransitionTo(nextStatus);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(RentalStatus.Returned, true)]
    [InlineData(RentalStatus.Completed, false)]
    [InlineData(RentalStatus.Approved, false)]
    public void OutForRentState_CanTransitionToExpectedStatuses(RentalStatus nextStatus, bool expected)
    {
        // Arrange
        IRentalState state = new OutForRentState();

        // Act
        var result = state.CanTransitionTo(nextStatus);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(RentalStatus.Completed, true)]
    [InlineData(RentalStatus.Returned, false)]
    [InlineData(RentalStatus.Requested, false)]
    public void ReturnedState_CanTransitionToExpectedStatuses(RentalStatus nextStatus, bool expected)
    {
        // Arrange
        IRentalState state = new ReturnedState();

        // Act
        var result = state.CanTransitionTo(nextStatus);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(RentalStatus.Requested)]
    [InlineData(RentalStatus.Approved)]
    [InlineData(RentalStatus.Completed)]
    public void RejectedState_CannotTransitionToAnyStatus(RentalStatus nextStatus)
    {
        // Arrange
        IRentalState state = new RejectedState();

        // Act
        var result = state.CanTransitionTo(nextStatus);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(RentalStatus.Requested)]
    [InlineData(RentalStatus.Approved)]
    [InlineData(RentalStatus.Completed)]
    public void CompletedState_CannotTransitionToAnyStatus(RentalStatus nextStatus)
    {
        // Arrange
        IRentalState state = new CompletedState();

        // Act
        var result = state.CanTransitionTo(nextStatus);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateTransitionTo_WhenTransitionIsAllowed_DoesNotThrow()
    {
        // Arrange
        IRentalState state = new RequestedState();

        // Act
        var exception = Record.Exception(() =>
            state.ValidateTransitionTo(RentalStatus.Approved));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ValidateTransitionTo_WhenTransitionIsInvalid_ThrowsInvalidOperationException()
    {
        // Arrange
        IRentalState state = new CompletedState();

        // Act + Assert
        Assert.Throws<InvalidOperationException>(() =>
            state.ValidateTransitionTo(RentalStatus.Requested));
    }
}