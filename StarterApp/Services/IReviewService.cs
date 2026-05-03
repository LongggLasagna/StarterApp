using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Defines review-related business operations for items and completed rentals.
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Checks whether a user is allowed to review an item.
    /// </summary>
    /// <param name="item">The item being reviewed.</param>
    /// <param name="reviewer">The user attempting to submit the review.</param>
    /// <returns>True if the user can review the item; otherwise false.</returns>
    Task<bool> CanReviewAsync(Item item, User reviewer);

    /// <summary>
    /// Submits a rating and comment for an item.
    /// </summary>
    /// <param name="item">The item being reviewed.</param>
    /// <param name="reviewer">The user submitting the review.</param>
    /// <param name="rating">The rating value, usually from 1 to 5.</param>
    /// <param name="comment">The review comment.</param>
    Task SubmitReviewAsync(Item item, User reviewer, int rating, string comment);

    /// <summary>
    /// Retrieves all reviews for a specific item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>A list of reviews for the item.</returns>
    Task<List<Review>> GetReviewsForItemAsync(int itemId);

    /// <summary>
    /// Calculates the average rating for a specific item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>The average item rating, or zero if no reviews exist.</returns>
    Task<double> GetAverageRatingForItemAsync(int itemId);
}