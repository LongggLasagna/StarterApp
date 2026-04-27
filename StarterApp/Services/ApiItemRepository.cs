using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Services;

public class ApiItemRepository : IItemRepository
{
    private readonly IApiService _apiService;

    public ApiItemRepository(IApiService apiService)
    {
        _apiService = apiService;
    }

    public Task<List<Item>> GetAllAsync()
    {
        return _apiService.GetItemsAsync();
    }

    public Task<Item?> GetByIdAsync(int id)
    {
        return _apiService.GetItemAsync(id);
    }

    public async Task AddAsync(Item item)
    {
        await _apiService.CreateItemAsync(item);
    }

    public async Task UpdateAsync(Item item)
    {
        await _apiService.UpdateItemAsync(item);
    }
}
