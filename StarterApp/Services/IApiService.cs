using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IApiService
{
    Task<List<Item>> GetItemsAsync(string? category = null, string? search = null, int page = 1);
    Task<Item?> GetItemAsync(int id);
    Task<Item> CreateItemAsync(Item item);
    Task<Item> UpdateItemAsync(Item item);

    Task<List<Review>> GetItemReviewsAsync(int itemId);
}
