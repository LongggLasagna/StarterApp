using StarterApp.Database.Models;

namespace StarterApp.Database.States;

/// <summary>
/// Final state used when a rental request has been rejected.
/// </summary>
public class RejectedState : IRentalState
{
    public RentalStatus Status => RentalStatus.Rejected;

    public bool CanTransitionTo(RentalStatus nextStatus)
    {
        return false;
    }
}
