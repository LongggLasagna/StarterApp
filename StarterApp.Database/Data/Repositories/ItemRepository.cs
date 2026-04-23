using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;


namespace StarterApp.Database.Data.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> GetAllAsync()
    {
        return await _context.Items
            .Include(i => i.Owner)
            .OrderBy(i => i.Title)
            .ToListAsync();
    }

    public async Task AddAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
    }
}