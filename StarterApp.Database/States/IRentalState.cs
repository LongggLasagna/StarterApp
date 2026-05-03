using StarterApp.Database.Models;

namespace StarterApp.Database.States;

/// <summary>
/// Defines behaviour for a rental workflow state.
/// Each state decides which rental status it can move to next.
/// </summary>
public interface IRentalState
{
    /// <summary>
    /// The rental status represented by this state.
    /// </summary>
    RentalStatus Status { get; }

    /// <summary>
    /// Checks whether this state can move to the requested next status.
    /// </summary>
    bool CanTransitionTo(RentalStatus nextStatus);

    /// <summary>
    /// Validates a state transition and throws an exception if it is not allowed.
    /// </summary>
    void ValidateTransitionTo(RentalStatus nextStatus)
    {
        if (!CanTransitionTo(nextStatus))
        {
            throw new InvalidOperationException(
                $"Cannot transition from {Status} to {nextStatus}.");
        }
    }
}