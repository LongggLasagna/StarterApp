using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Database.States;

namespace StarterApp.Services;

/// <summary>
/// Contains business logic for rental pricing and rental workflow transitions.
/// </summary>
public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;

    public RentalService(IRentalRepository rentalRepository)
    {
        _rentalRepository = rentalRepository;
    }

/// <summary>
/// Calculates the total rental price based on item daily rate and rental duration.
/// </summary>
/// 
    public decimal CalculateTotalPrice(Item item, DateTime startDate, DateTime endDate)
    {
        var days = (endDate.Date - startDate.Date).Days;

        if (days <= 0)
        {
            days = 1;
        }

        return days * item.DailyRate;
    }

/// <summary>
/// Creates a new rental request after validating ownership, dates, and overlapping rentals.
/// </summary>
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

    private static IRentalState GetState(Rental rental)
    {
    return rental.Status switch
        {
            RentalStatus.Requested => new RequestedState(),
            RentalStatus.Approved => new ApprovedState(),
            RentalStatus.Rejected => new RejectedState(),
            RentalStatus.OutForRent => new OutForRentState(),
            RentalStatus.Returned => new ReturnedState(),
            RentalStatus.Completed => new CompletedState(),
            _ => new RequestedState()
        };
    }

    public async Task MarkOutForRentAsync(Rental rental)
    {
        var state = GetState(rental);
        state.ValidateTransitionTo(RentalStatus.OutForRent);

        rental.Status = RentalStatus.OutForRent;
        await _rentalRepository.UpdateAsync(rental);
    }

    public async Task MarkReturnedAsync(Rental rental)
    {
        var state = GetState(rental);
        state.ValidateTransitionTo(RentalStatus.Returned);

        rental.Status = RentalStatus.Returned;
        await _rentalRepository.UpdateAsync(rental);
    }

    public async Task MarkCompletedAsync(Rental rental)
    {
        var state = GetState(rental);
        state.ValidateTransitionTo(RentalStatus.Completed);

        rental.Status = RentalStatus.Completed;
        await _rentalRepository.UpdateAsync(rental);
    }

    public async Task ApproveRentalAsync(Rental rental)
    {
        var state = GetState(rental);
        state.ValidateTransitionTo(RentalStatus.Approved);

        rental.Status = RentalStatus.Approved;
        await _rentalRepository.UpdateAsync(rental);
    }

    public async Task RejectRentalAsync(Rental rental)
    {
        var state = GetState(rental);
        state.ValidateTransitionTo(RentalStatus.Rejected);

        rental.Status = RentalStatus.Rejected;
        await _rentalRepository.UpdateAsync(rental);
    }
}
