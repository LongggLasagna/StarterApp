using System.Net.Http.Json;
using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Provides API communication between the MAUI application and the shared rental backend.
/// Handles item discovery, item management, nearby search, and review requests.
/// </summary>
public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Creates a new API service using the configured HTTP client.
    /// </summary>
    /// <param name="httpClient">The HTTP client configured with the backend base URL.</param>
    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves a paginated list of items from the backend API.
    /// Optional category and search filters can be supplied.
    /// </summary>
    /// <param name="category">Optional item category filter.</param>
    /// <param name="search">Optional text search filter.</param>
    /// <param name="page">The page number to request.</param>
    /// <returns>A list of items mapped into the application model.</returns>
    public async Task<List<Item>> GetItemsAsync(string? category = null, string? search = null, int page = 1)
    {
        var query = $"/items?page={page}&pageSize=20";

        if (!string.IsNullOrWhiteSpace(category))
            query += $"&category={Uri.EscapeDataString(category)}";

        if (!string.IsNullOrWhiteSpace(search))
            query += $"&search={Uri.EscapeDataString(search)}";

        var response = await _httpClient.GetFromJsonAsync<ApiItemsResponse>(query);

        return response?.Items.Select(ToItem).ToList() ?? new List<Item>();
    }

    /// <summary>
    /// Retrieves items near a latitude and longitude using the backend nearby search endpoint.
    /// This supports the location-based discovery feature.
    /// </summary>
    /// <param name="latitude">The latitude of the search centre.</param>
    /// <param name="longitude">The longitude of the search centre.</param>
    /// <param name="radiusKm">The search radius in kilometres.</param>
    /// <returns>A list of nearby items.</returns>
    public async Task<List<Item>> GetNearbyItemsAsync(double latitude, double longitude, double radiusKm)
    {
        var query = $"/items/nearby?lat={latitude}&lon={longitude}&radius={radiusKm}";

        var response = await _httpClient.GetFromJsonAsync<ApiNearbyItemsResponse>(query);

        return response?.items.Select(ToItem).ToList() ?? new List<Item>();
    }

    /// <summary>
    /// Retrieves detailed information for a single item by its identifier.
    /// </summary>
    /// <param name="id">The item identifier.</param>
    /// <returns>The item if found; otherwise null.</returns>
    public async Task<Item?> GetItemAsync(int id)
    {
        var dto = await _httpClient.GetFromJsonAsync<ApiItemDto>($"items/{id}");
        return dto == null ? null : ToItem(dto);
    }

    /// <summary>
    /// Creates a new item listing through the backend API.
    /// </summary>
    /// <param name="item">The item details entered by the user.</param>
    /// <returns>The created item returned by the API.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the API rejects the item creation request.</exception>
    public async Task<Item> CreateItemAsync(Item item)
    {
        var request = new ApiCreateItemRequest(
            item.Title,
            item.Description,
            item.DailyRate,
            item.CategoryId,
            item.Latitude,
            item.Longitude);

        var response = await _httpClient.PostAsJsonAsync("items", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new InvalidOperationException(error?.Message ?? "Failed to create item.");
        }

        var created = await response.Content.ReadFromJsonAsync<ApiItemDto>();
        return ToItem(created!);
    }

    /// <summary>
    /// Updates an existing item listing through the backend API.
    /// </summary>
    /// <param name="item">The item containing updated values.</param>
    /// <returns>The updated item returned by the API.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the API rejects the update request.</exception>
    public async Task<Item> UpdateItemAsync(Item item)
    {
        var request = new ApiUpdateItemRequest(
            item.Title,
            item.Description,
            item.DailyRate,
            true);

        var response = await _httpClient.PutAsJsonAsync($"items/{item.Id}", request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new InvalidOperationException(error?.Message ?? "Failed to update item.");
        }

        var updated = await response.Content.ReadFromJsonAsync<ApiItemDto>();
        return updated == null ? item : ToItem(updated);
    }

    /// <summary>
    /// Maps an API item data transfer object into the local application item model.
    /// </summary>
    /// <param name="dto">The API item data transfer object.</param>
    /// <returns>The mapped item model.</returns>
    private static Item ToItem(ApiItemDto dto)
    {
        return new Item
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            DailyRate = dto.DailyRate,
            CategoryId = dto.CategoryId,
            Category = dto.Category,
            OwnerId = dto.OwnerId,
            Latitude = dto.Latitude ?? 0,
            Longitude = dto.Longitude ?? 0,
            IsAvailable = dto.IsAvailable,
            Location = dto.Latitude.HasValue && dto.Longitude.HasValue
                ? $"{dto.Latitude:F4}, {dto.Longitude:F4}"
                : "API location"
        };
    }

    /// <summary>
    /// Retrieves reviews for a specific item from the backend API.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>A list of reviews for the item.</returns>
    public async Task<List<Review>> GetItemReviewsAsync(int itemId)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiItemReviewsResponse>($"items/{itemId}/reviews");

        return response?.reviews.Select(r => new Review
        {
            Id = r.Id,
            ReviewerId = r.ReviewerId,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt
        }).ToList() ?? new List<Review>();
    }

    /// <summary>
    /// Creates a review for a completed rental through the backend API.
    /// </summary>
    /// <param name="rentalId">The completed rental identifier.</param>
    /// <param name="rating">The star rating from 1 to 5.</param>
    /// <param name="comment">The optional review comment.</param>
    /// <returns>The created review returned by the API.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the API rejects the review request.</exception>
    public async Task<Review> CreateReviewAsync(int rentalId, int rating, string comment)
    {
        var response = await _httpClient.PostAsJsonAsync("reviews", new
        {
            rentalId,
            rating,
            comment
        });

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to submit review: {body}");
        }

        var dto = await response.Content.ReadFromJsonAsync<ApiReviewDto>();

        return new Review
        {
            Id = dto!.Id,
            ReviewerId = dto.ReviewerId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedAt = dto.CreatedAt
        };
    }
}