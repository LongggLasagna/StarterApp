using StarterApp.Database.Models;

namespace StarterApp.Database.States;

/// <summary>
/// State used when the borrower has returned the item and owner confirmation is required.
/// </summary>
public class ReturnedState : IRentalState
{
    public RentalStatus Status => RentalStatus.Returned;

    public bool CanTransitionTo(RentalStatus nextStatus)
    {
        return nextStatus == RentalStatus.Completed;
    }
}