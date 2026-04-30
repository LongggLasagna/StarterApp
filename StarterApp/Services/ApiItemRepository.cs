using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// API-backed implementation of the item repository.
/// This class adapts the repository pattern to the shared backend API,
/// allowing ViewModels and services to depend on IItemRepository rather than direct HTTP calls.
/// </summary>
public class ApiItemRepository : IItemRepository
{
    private readonly IApiService _apiService;

    /// <summary>
    /// Creates a new API item repository.
    /// </summary>
    /// <param name="apiService">The API service used to communicate with the backend.</param>
    public ApiItemRepository(IApiService apiService)
    {
        _apiService = apiService;
    }

    /// <summary>
    /// Retrieves all available items from the backend API.
    /// </summary>
    /// <returns>A list of available item listings.</returns>
    public async Task<List<Item>> GetAllAsync()
    {
        var items = await _apiService.GetItemsAsync();

        Console.WriteLine($"[API REPO] Got {items.Count} items");

        return items;
    }

    /// <summary>
    /// Retrieves a single item by its identifier.
    /// </summary>
    /// <param name="id">The item identifier.</param>
    /// <returns>The matching item if found; otherwise null.</returns>
    public Task<Item?> GetByIdAsync(int id)
    {
        return _apiService.GetItemAsync(id);
    }

    /// <summary>
    /// Creates a new item listing through the backend API.
    /// </summary>
    /// <param name="item">The item to create.</param>
    public async Task AddAsync(Item item)
    {
        await _apiService.CreateItemAsync(item);
    }

    /// <summary>
    /// Updates an existing item listing through the backend API.
    /// </summary>
    /// <param name="item">The item containing updated details.</param>
    public async Task UpdateAsync(Item item)
    {
        await _apiService.UpdateItemAsync(item);
    }
}