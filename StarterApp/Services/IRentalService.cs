using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IRentalService
{
    decimal CalculateTotalPrice(Item item, DateTime startDate, DateTime endDate);

    Task<Rental> RequestRentalAsync(Item item, User borrower, DateTime startDate, DateTime endDate);

    Task ApproveRentalAsync(Rental rental);
    Task RejectRentalAsync(Rental rental);

    Task MarkOutForRentAsync(Rental rental);
    Task MarkReturnedAsync(Rental rental);
    Task MarkCompletedAsync(Rental rental);
}
