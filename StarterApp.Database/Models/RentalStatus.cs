namespace StarterApp.Database.Models;

/// <summary>
/// Represents the lifecycle state of a rental request.
/// Used to control which rental actions are available in the UI and API workflow.
/// </summary>
public enum RentalStatus
{
    /// <summary>
    /// The borrower has requested the rental and is waiting for the owner to respond.
    /// </summary>
    Requested,

    /// <summary>
    /// The owner has approved the rental request.
    /// </summary>
    Approved,

    /// <summary>
    /// The owner has rejected the rental request.
    /// </summary>
    Rejected,

    /// <summary>
    /// The item is currently out with the borrower.
    /// </summary>
    OutForRent,

    /// <summary>
    /// The borrower has marked the item as returned.
    /// </summary>
    Returned,

    /// <summary>
    /// The owner has confirmed the return and completed the rental.
    /// </summary>
    Completed
}