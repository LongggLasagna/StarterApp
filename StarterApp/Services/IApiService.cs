using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Defines API operations used by the application to communicate with the shared backend.
/// </summary>
public interface IApiService
{
     /// <summary>
    /// Retrieves a paginated list of available items from the backend API.
    /// </summary>
    Task<List<Item>> GetItemsAsync(string? category = null, string? search = null, int page = 1);
    /// <summary>
    /// Retrieves nearby items using latitude, longitude, and a configurable search radius.
    /// </summary>

    Task<List<Item>> GetNearbyItemsAsync(double latitude, double longitude, double radiusKm);
    /// <summary>
    /// Retrieves the full details for a single item by ID.
    /// </summary>
    Task<Item?> GetItemAsync(int id);
    /// <summary>
    /// Creates a new item listing through the backend API.
    /// </summary>
    Task<Item> CreateItemAsync(Item item);
    /// <summary>
    /// Updates an existing item listing through the backend API.
    /// </summary>
    Task<Item> UpdateItemAsync(Item item);
    /// <summary>
    /// Submits a review for a completed rental.
    /// </summary>
    Task<Review> CreateReviewAsync(int rentalId, int rating, string comment);
     /// <summary>
    /// Retrieves reviews for a specific item.
    /// </summary>
    Task<List<Review>> GetItemReviewsAsync(int itemId);


}

