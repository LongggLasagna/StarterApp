using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IReviewService
{
    Task SubmitReviewAsync(Item item, User reviewer, int rating, string comment);
    Task<List<Review>> GetReviewsForItemAsync(int itemId);
    Task<double> GetAverageRatingForItemAsync(int itemId);
}