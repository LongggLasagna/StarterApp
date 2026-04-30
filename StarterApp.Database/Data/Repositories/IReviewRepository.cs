using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public interface IReviewRepository
{
    Task AddAsync(Review review);
    Task<List<Review>> GetForItemAsync(int itemId);
    Task<double> GetAverageRatingForItemAsync(int itemId);
}
