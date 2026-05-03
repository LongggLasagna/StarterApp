using StarterApp.Database.Models;

namespace StarterApp.Database.States;

/// <summary>
/// State used when a rental request has been approved by the owner.
/// </summary>
public class ApprovedState : IRentalState
{
    public RentalStatus Status => RentalStatus.Approved;

    public bool CanTransitionTo(RentalStatus nextStatus)
    {
        return nextStatus == RentalStatus.OutForRent;
    }
}