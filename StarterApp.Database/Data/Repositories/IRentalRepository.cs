using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

public interface IRentalRepository
{
    Task AddAsync(Rental rental);
    Task<List<Rental>> GetOutgoingAsync(int borrowerId);
    Task<List<Rental>> GetIncomingAsync(int ownerId);
}