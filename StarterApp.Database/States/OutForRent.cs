using StarterApp.Database.Models;

namespace StarterApp.Database.States;

/// <summary>
/// State used when the item is currently out with the borrower.
/// </summary>
public class OutForRentState : IRentalState
{
    public RentalStatus Status => RentalStatus.OutForRent;

    public bool CanTransitionTo(RentalStatus nextStatus)
    {
        return nextStatus == RentalStatus.Returned;
    }
}
