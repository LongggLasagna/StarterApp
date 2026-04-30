using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;

    public RentalService(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

    public decimal CalculateTotalPrice(Item item, DateTime startDate, DateTime endDate)
    {
        var days = (endDate.Date - startDate.Date).Days;

        if (days <= 0)
        {
            days = 1;
        }

        return days * item.DailyRate;
    }

    public async Task<Rental> RequestRentalAsync(Item item, User borrower, DateTime startDate, DateTime endDate)
    {
        if (borrower.Id == item.OwnerId)
        {
            throw new InvalidOperationException("You cannot request to rent your own item.");
        }

        if (endDate.Date < startDate.Date)
        {
            throw new InvalidOperationException("End date cannot be before start date.");
        }

        var hasOverlap = await _rentalRepository.HasOverLappingRentalAsync(item.Id, startDate, endDate);
        if (hasOverlap)
        {
            throw new InvalidOperationException("The item is already rented for the selected dates.");
        }


        var utcStartDate = DateTime.SpecifyKind(startDate.Date, DateTimeKind.Utc);
        var utcEndDate = DateTime.SpecifyKind(endDate.Date, DateTimeKind.Utc);

       

        var rental = new Rental
        {
            ItemId = item.Id,
            BorrowerId = borrower.Id,
            StartDate = utcStartDate,
            EndDate = utcEndDate,
            TotalPrice = CalculateTotalPrice(item, startDate, endDate),
            Status = RentalStatus.Requested
        };

        await _rentalRepository.AddAsync(rental);

        return rental;
    }

    public async Task MarkOutForRentAsync(Rental rental)
    {
        if (rental.Status != RentalStatus.Approved)
            throw new InvalidOperationException("Rental must be approved first.");

        rental.Status = RentalStatus.OutForRent;
        await _rentalRepository.UpdateAsync(rental);
    }

    public async Task MarkReturnedAsync(Rental rental)
    {
        if (rental.Status != RentalStatus.OutForRent)
            throw new InvalidOperationException("Rental must be out for rent.");

        rental.Status = RentalStatus.Returned;
        await _rentalRepository.UpdateAsync(rental);
    }

    public async Task MarkCompletedAsync(Rental rental)
    {
        if (rental.Status != RentalStatus.Returned)
            throw new InvalidOperationException("Rental must be returned first.");

        rental.Status = RentalStatus.Completed;
        await _rentalRepository.UpdateAsync(rental);
    }

    public async Task ApproveRentalAsync(Rental rental)
    {
        if (rental.Status != RentalStatus.Requested)
        {
            throw new InvalidOperationException("Only requested rentals can be approved.");
        }

        rental.Status = RentalStatus.Approved;
        await _rentalRepository.UpdateAsync(rental);
    }

    public async Task RejectRentalAsync(Rental rental)
    {
        if (rental.Status != RentalStatus.Requested)
        {
            throw new InvalidOperationException("Only requested rentals can be rejected.");
        }

        rental.Status = RentalStatus.Rejected;
        await _rentalRepository.UpdateAsync(rental);
    }
}
