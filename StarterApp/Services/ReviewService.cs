using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Contains business logic for item reviews.
/// Validates review eligibility, rating values, and comments before saving reviews.
/// </summary>
public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IRentalRepository _rentalRepository;

    /// <summary>
    /// Creates a new review service.
    /// </summary>
    /// <param name="reviewRepository">Repository used to store and retrieve reviews.</param>
    /// <param name="rentalRepository">Repository used to check completed rental history.</param>
    public ReviewService(
        IReviewRepository reviewRepository,
        IRentalRepository rentalRepository)
    {
        _reviewRepository = reviewRepository;
        _rentalRepository = rentalRepository;
    }

    /// <summary>
    /// Checks whether a reviewer is allowed to review an item.
    /// A review is only allowed after the reviewer has completed a rental for the item.
    /// </summary>
    /// <param name="item">The item being reviewed.</param>
    /// <param name="reviewer">The user attempting to review the item.</param>
    /// <returns>True if the reviewer has completed a rental for the item; otherwise false.</returns>
    public async Task<bool> CanReviewAsync(Item item, User reviewer)
    {
        return await _rentalRepository.HasCompletedRentalAsync(item.Id, reviewer.Id);
    }

    /// <summary>
    /// Submits a review after validating rating, comment, and rental completion.
    /// </summary>
    /// <param name="item">The item being reviewed.</param>
    /// <param name="reviewer">The user submitting the review.</param>
    /// <param name="rating">The rating value from 1 to 5.</param>
    /// <param name="comment">The review comment.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the rating is invalid, the comment is empty, or the user is not eligible to review.
    /// </exception>
    public async Task SubmitReviewAsync(Item item, User reviewer, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new InvalidOperationException("Rating must be between 1 and 5.");

        if (string.IsNullOrWhiteSpace(comment))
            throw new InvalidOperationException("Comment is required.");

        if (!await CanReviewAsync(item, reviewer))
            throw new InvalidOperationException("You can only review items after completing a rental.");

        var review = new Review
        {
            ItemId = item.Id,
            ReviewerId = reviewer.Id,
            Rating = rating,
            Comment = comment.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await _reviewRepository.AddAsync(review);
    }

    /// <summary>
    /// Retrieves all reviews for a specific item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>A list of reviews for the item.</returns>
    public Task<List<Review>> GetReviewsForItemAsync(int itemId)
    {
        return _reviewRepository.GetForItemAsync(itemId);
    }

    /// <summary>
    /// Gets the average rating for a specific item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>The average rating for the item, or zero if no reviews exist.</returns>
    public Task<double> GetAverageRatingForItemAsync(int itemId)
    {
        return _reviewRepository.GetAverageRatingForItemAsync(itemId);
    }
}