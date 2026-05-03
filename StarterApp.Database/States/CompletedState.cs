using StarterApp.Database.Models;

namespace StarterApp.Database.States;

/// <summary>
/// Final state used when the rental has been fully completed.
/// </summary>
public class CompletedState : IRentalState
{
    public RentalStatus Status => RentalStatus.Completed;

    public bool CanTransitionTo(RentalStatus nextStatus)
    {
        return false;
    }
}