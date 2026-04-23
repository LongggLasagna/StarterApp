using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public interface IItemRepository
{
    Task<List<Item>> GetAllAsync();
    Task AddAsync(Item item);
}

