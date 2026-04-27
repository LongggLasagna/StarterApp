using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task SubmitReviewAsync(Item item, User reviewer, int rating, string comment)
    {
        if (rating < 1 || rating > 5)
        {
            throw new InvalidOperationException("Rating must be between 1 and 5.");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException("Comment is required.");
        }

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

    public Task<List<Review>> GetReviewsForItemAsync(int itemId)
    {
        return _reviewRepository.GetForItemAsync(itemId);
    }

    public Task<double> GetAverageRatingForItemAsync(int itemId)
    {
        return _reviewRepository.GetAverageRatingForItemAsync(itemId);
    }
}
