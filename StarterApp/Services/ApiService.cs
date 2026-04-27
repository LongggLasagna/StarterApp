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
        var query = $"items?page={page}&pageSize=20";

        if (!string.IsNullOrWhiteSpace(category))
            query += $"&category={Uri.EscapeDataString(category)}";

        if (!string.IsNullOrWhiteSpace(search))
            query += $"&search={Uri.EscapeDataString(search)}";

        var response = await _httpClient.GetFromJsonAsync<ApiItemsResponse>(query);

        return response?.Items.Select(ToItem).ToList() ?? new List<Item>();
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
            1,
            55.9533,
            -3.1883);

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
            Category = dto.Category,
            Location = dto.Latitude.HasValue && dto.Longitude.HasValue
                ? $"{dto.Latitude:F4}, {dto.Longitude:F4}"
                : "API location",
            OwnerId = dto.OwnerId
        };
    }
}