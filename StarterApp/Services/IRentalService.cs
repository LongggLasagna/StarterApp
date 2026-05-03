using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Defines business logic operations for rental pricing and rental workflow management.
/// </summary>
public interface IRentalService
{
    /// <summary>
    /// Calculates the total rental price using the item's daily rate and rental duration.
    /// </summary>
    /// <param name="item">The item being rented.</param>
    /// <param name="startDate">The rental start date.</param>
    /// <param name="endDate">The rental end date.</param>
    /// <returns>The total rental price.</returns>
    decimal CalculateTotalPrice(Item item, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Creates a rental request after validating ownership, date range, and overlapping rentals.
    /// </summary>
    /// <param name="item">The item being requested.</param>
    /// <param name="borrower">The user requesting the rental.</param>
    /// <param name="startDate">The requested start date.</param>
    /// <param name="endDate">The requested end date.</param>
    /// <returns>The created rental request.</returns>
    Task<Rental> RequestRentalAsync(Item item, User borrower, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Approves a rental request.
    /// </summary>
    /// <param name="rental">The rental to approve.</param>
    Task ApproveRentalAsync(Rental rental);

    /// <summary>
    /// Rejects a rental request.
    /// </summary>
    /// <param name="rental">The rental to reject.</param>
    Task RejectRentalAsync(Rental rental);

    /// <summary>
    /// Moves an approved rental into the out-for-rent state.
    /// </summary>
    /// <param name="rental">The approved rental.</param>
    Task MarkOutForRentAsync(Rental rental);

    /// <summary>
    /// Marks an out-for-rent rental as returned.
    /// </summary>
    /// <param name="rental">The rental being returned.</param>
    Task MarkReturnedAsync(Rental rental);

    /// <summary>
    /// Completes a returned rental after owner confirmation.
    /// </summary>
    /// <param name="rental">The returned rental.</param>
    Task MarkCompletedAsync(Rental rental);
}
