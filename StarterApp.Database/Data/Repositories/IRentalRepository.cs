using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

/// <summary>
/// Defines data access operations for rental requests and rental workflow state.
/// </summary>
public interface IRentalRepository
{
    /// <summary>
    /// Adds a new rental request to the data store.
    /// </summary>
    /// <param name="rental">The rental request to add.</param>
    Task AddAsync(Rental rental);

    /// <summary>
    /// Retrieves a rental by its identifier.
    /// </summary>
    /// <param name="id">The rental identifier.</param>
    /// <returns>The matching rental if found; otherwise null.</returns>
    Task<Rental?> GetByIdAsync(int id);

    /// <summary>
    /// Updates an existing rental, usually after a status change.
    /// </summary>
    /// <param name="rental">The rental containing updated values.</param>
    Task UpdateAsync(Rental rental);

    /// <summary>
    /// Retrieves rentals requested by a borrower.
    /// </summary>
    /// <param name="borrowerId">The borrower user identifier.</param>
    /// <returns>A list of outgoing rental requests.</returns>
    Task<List<Rental>> GetOutgoingAsync(int borrowerId);

    /// <summary>
    /// Retrieves rental requests for items owned by a user.
    /// </summary>
    /// <param name="ownerId">The owner user identifier.</param>
    /// <returns>A list of incoming rental requests.</returns>
    Task<List<Rental>> GetIncomingAsync(int ownerId);

    /// <summary>
    /// Checks whether an item has an overlapping approved rental for the requested dates.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="startDate">The proposed rental start date.</param>
    /// <param name="endDate">The proposed rental end date.</param>
    /// <returns>True if an overlapping rental exists; otherwise false.</returns>
    Task<bool> HasOverLappingRentalAsync(int itemId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Checks whether a borrower has completed a rental for a specific item.
    /// Used to determine review eligibility.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="borrowerId">The borrower user identifier.</param>
    /// <returns>True if a completed rental exists; otherwise false.</returns>
    Task<bool> HasCompletedRentalAsync(int itemId, int borrowerId);
}