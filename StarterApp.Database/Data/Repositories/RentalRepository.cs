using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

/// <summary>
/// Entity Framework implementation of the rental repository.
/// Handles storing rentals, retrieving incoming and outgoing requests, and checking rental rules.
/// </summary>
public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Creates a new rental repository using the application database context.
    /// </summary>
    /// <param name="context">The Entity Framework database context.</param>
    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new rental request to the database.
    /// </summary>
    /// <param name="rental">The rental request to save.</param>
    public async Task AddAsync(Rental rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves rentals requested by a specific borrower.
    /// </summary>
    /// <param name="borrowerId">The borrower user identifier.</param>
    /// <returns>A list of outgoing rentals ordered by creation date.</returns>
    public async Task<List<Rental>> GetOutgoingAsync(int borrowerId)
    {
        return await _context.Rentals
            .Include(r => r.Item)
            .Where(r => r.BorrowerId == borrowerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves rental requests for items owned by a specific user.
    /// </summary>
    /// <param name="ownerId">The owner user identifier.</param>
    /// <returns>A list of incoming rentals ordered by creation date.</returns>
    public async Task<List<Rental>> GetIncomingAsync(int ownerId)
    {
        return await _context.Rentals
            .Include(r => r.Item)
            .Include(r => r.Borrower)
            .Where(r => r.Item != null && r.Item.OwnerId == ownerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a rental by its identifier.
    /// Includes the related item and borrower for display and workflow logic.
    /// </summary>
    /// <param name="id">The rental identifier.</param>
    /// <returns>The matching rental if found; otherwise null.</returns>
    public async Task<Rental?> GetByIdAsync(int id)
    {
        return await _context.Rentals
            .Include(r => r.Item)
            .Include(r => r.Borrower)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Updates an existing rental in the database.
    /// </summary>
    /// <param name="rental">The rental containing updated values.</param>
    public async Task UpdateAsync(Rental rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Checks whether an item already has an approved rental overlapping the requested date range.
    /// This prevents double-booking for the same item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="startDate">The requested start date.</param>
    /// <param name="endDate">The requested end date.</param>
    /// <returns>True if an overlapping approved rental exists; otherwise false.</returns>
    public async Task<bool> HasOverLappingRentalAsync(int itemId, DateTime startDate, DateTime endDate)
    {
        var utcStart = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
        var utcEnd = DateTime.SpecifyKind(endDate.Date, DateTimeKind.Utc);

        return await _context.Rentals.AnyAsync(r =>
            r.ItemId == itemId &&
            r.Status == RentalStatus.Approved &&
            (
                utcStart < r.EndDate && utcEnd > r.StartDate
            )
        );
    }

    /// <summary>
    /// Checks whether a borrower has completed a rental for a specific item.
    /// This is used to determine whether the borrower can leave a review.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="borrowerId">The borrower user identifier.</param>
    /// <returns>True if a completed rental exists; otherwise false.</returns>
    public async Task<bool> HasCompletedRentalAsync(int itemId, int borrowerId)
    {
        return await _context.Rentals.AnyAsync(r =>
            r.ItemId == itemId &&
            r.BorrowerId == borrowerId &&
            r.Status == RentalStatus.Completed);
    }
}