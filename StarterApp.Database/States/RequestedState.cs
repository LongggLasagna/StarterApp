using StarterApp.Database.Models;

namespace StarterApp.Database.States;

/// <summary>
/// State used when a rental has been requested and is waiting for owner approval.
/// </summary>
public class RequestedState : IRentalState
{
    public RentalStatus Status => RentalStatus.Requested;

    public bool CanTransitionTo(RentalStatus nextStatus)
    {
        return nextStatus == RentalStatus.Approved ||
               nextStatus == RentalStatus.Rejected;
    }
}