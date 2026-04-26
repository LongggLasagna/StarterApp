using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public interface IRentalRepository
{
    Task AddAsync(Rental rental);
    Task<Rental?> GetByIdAsync(int id);
    Task UpdateAsync(Rental rental);
    Task<List<Rental>> GetOutgoingAsync(int borrowerId);
    Task<List<Rental>> GetIncomingAsync(int ownerId);
    Task<bool> HasOverLappingRentalAsync(int itemId, DateTime startDate, DateTime endDate);
  
}