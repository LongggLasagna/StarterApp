using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

/// <summary>
/// Entity Framework implementation of the review repository.
/// Handles storing reviews, retrieving item reviews, and calculating average item ratings.
/// </summary>
public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Creates a new review repository using the application database context.
    /// </summary>
    /// <param name="context">The Entity Framework database context.</param>
    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new review to the database.
    /// </summary>
    /// <param name="review">The review to save.</param>
    public async Task AddAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves all reviews for a specific item, ordered newest first.
    /// Reviewer details are included for display purposes.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>A list of reviews for the item.</returns>
    public async Task<List<Review>> GetForItemAsync(int itemId)
    {
        return await _context.Reviews
            .Include(r => r.Reviewer)
            .Where(r => r.ItemId == itemId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Calculates the average rating for a specific item.
    /// Returns zero when the item has no reviews.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>The average rating for the item.</returns>
    public async Task<double> GetAverageRatingForItemAsync(int itemId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.ItemId == itemId)
            .ToListAsync();

        if (!reviews.Any())
            return 0;

        return reviews.Average(r => r.Rating);
    }
}
