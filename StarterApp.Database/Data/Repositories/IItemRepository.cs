using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

/// <summary>
/// Defines data access operations for item listings.
/// </summary>
public interface IItemRepository
{
    /// <summary>
    /// Retrieves all item listings.
    /// </summary>
    /// <returns>A list of items.</returns>
    Task<List<Item>> GetAllAsync();

    /// <summary>
    /// Retrieves a single item by its identifier.
    /// </summary>
    /// <param name="id">The item identifier.</param>
    /// <returns>The matching item if found; otherwise null.</returns>
    Task<Item?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new item listing.
    /// </summary>
    /// <param name="item">The item to add.</param>
    Task AddAsync(Item item);

    /// <summary>
    /// Updates an existing item listing.
    /// </summary>
    /// <param name="item">The item containing updated values.</param>
    Task UpdateAsync(Item item);
}