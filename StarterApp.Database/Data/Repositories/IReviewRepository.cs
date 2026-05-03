using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

/// <summary>
/// Defines data access operations for item reviews.
/// </summary>
public interface IReviewRepository
{
    /// <summary>
    /// Adds a new review to the data store.
    /// </summary>
    /// <param name="review">The review to add.</param>
    Task AddAsync(Review review);

    /// <summary>
    /// Retrieves all reviews for a specific item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>A list of reviews for the item.</returns>
    Task<List<Review>> GetForItemAsync(int itemId);

    /// <summary>
    /// Calculates the average rating for a specific item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>The average rating for the item, or zero if there are no reviews.</returns>
    Task<double> GetAverageRatingForItemAsync(int itemId);
}
