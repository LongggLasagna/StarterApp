using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

/// <summary>
/// Entity Framework implementation of the item repository.
/// Handles local database access for item listings.
/// </summary>
public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Creates a new item repository using the application database context.
    /// </summary>
    /// <param name="context">The Entity Framework database context.</param>
    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all item listings from the local database, including owner details.
    /// </summary>
    /// <returns>A list of items ordered by title.</returns>
    public async Task<List<Item>> GetAllAsync()
    {
        return await _context.Items
            .Include(i => i.Owner)
            .OrderBy(i => i.Title)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a single item by its identifier, including owner details.
    /// </summary>
    /// <param name="id">The item identifier.</param>
    /// <returns>The matching item if found; otherwise null.</returns>
    public async Task<Item?> GetByIdAsync(int id)
    {
        return await _context.Items
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    /// <summary>
    /// Adds a new item listing to the local database.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public async Task AddAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing item listing in the local database.
    /// </summary>
    /// <param name="item">The item containing updated values.</param>
    public async Task UpdateAsync(Item item)
    {
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
    }
}