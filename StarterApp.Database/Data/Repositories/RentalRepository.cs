using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Rental rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Rental>> GetOutgoingAsync(int borrowerId)
    {
        return await _context.Rentals
            .Include(r => r.Item)
            .Where(r => r.BorrowerId == borrowerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Rental>> GetIncomingAsync(int ownerId)
    {
        return await _context.Rentals
            .Include(r => r.Item)
            .Include(r => r.Borrower)
            .Where(r => r.Item != null && r.Item.OwnerId == ownerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Rental?> GetByIdAsync(int id)
    {
        return await _context.Rentals
            .Include(r => r.Item)
            .Include(r => r.Borrower)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task UpdateAsync(Rental rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasOverLappingRentalAsync(int itemId, DateTime startDate, DateTime endDate)
    {
        var utcStart = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
        var utcEnd = DateTime.SpecifyKind(endDate.Date, DateTimeKind.Utc);

        return await _context.Rentals.AnyAsync(r =>
            r.ItemId == itemId &&
            r.Status == RentalStatus.Approved &&
            (
                utcStart < r.EndDate && utcEnd > r.StartDate
            )
        );
    }
}