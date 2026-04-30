using System.Net.Http.Json;
using StarterApp.Database.Models;

namespace StarterApp.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

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
    public async Task<List<Item>> GetNearbyItemsAsync(double latitude, double longitude, double radiusKm)
        {
            var query = $"/items/nearby?lat={latitude}&lon={longitude}&radius={radiusKm}";

            var response = await _httpClient.GetFromJsonAsync<ApiNearbyItemsResponse>(query);

            return response?.items.Select(ToItem).ToList() ?? new List<Item>();
        }

    public async Task<Item?> GetItemAsync(int id)
    {
        var dto = await _httpClient.GetFromJsonAsync<ApiItemDto>($"items/{id}");
        return dto == null ? null : ToItem(dto);
    }

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


    //review get 
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