using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;


namespace StarterApp.ViewModels;

public partial class RentalsViewModel : BaseViewModel
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IAuthenticationService _authService;
    
    public ObservableCollection<Rental> OutgoingRentals { get; } = new();
    public ObservableCollection<Rental> IncomingRentals { get; } = new();

    public bool HasIncomingRentals => IncomingRentals.Any();
    public bool HasOutgoingRentals => OutgoingRentals.Any();

    public bool HasNoIncomingRentals => !HasIncomingRentals;
    public bool HasNoOutgoingRentals => !HasOutgoingRentals;


    public RentalsViewModel(
        IRentalRepository rentalRepository,
        IAuthenticationService authService)
    {
        _rentalRepository = rentalRepository;
        _authService = authService;

        Title = "RENTALS";
    }
    
    public async Task LoadRentalsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            IncomingRentals.Clear();
            OutgoingRentals.Clear();

            var user = _authService.CurrentUser;
            if (user == null)
            {
                SetError("You must be logged in.");
                return;
            }

            var incoming = await _rentalRepository.GetIncomingAsync(user.Id);
            var outgoing = await _rentalRepository.GetOutgoingAsync(user.Id);

            foreach (var rental in incoming)
                IncomingRentals.Add(rental);

            foreach (var rental in outgoing)
                OutgoingRentals.Add(rental);

            OnPropertyChanged(nameof(HasIncomingRentals));
            OnPropertyChanged(nameof(HasOutgoingRentals));
            OnPropertyChanged(nameof(HasNoIncomingRentals));
            OnPropertyChanged(nameof(HasNoOutgoingRentals));
        }
        catch (Exception ex)
        {
            SetError($"Failed to load rentals: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }

            
    }

    [RelayCommand]
    private async Task ApproveRentalAsync(Rental rental)
    {
        if (rental == null) return;

        rental.Status = RentalStatus.Approved;
        await _rentalRepository.UpdateAsync(rental);
        await LoadRentalsAsync();
    }

    [RelayCommand]
    private async Task RejectRentalAsync(Rental rental)
    {
        if (rental == null) return;

        rental.Status = RentalStatus.Rejected;
        await _rentalRepository.UpdateAsync(rental);
        await LoadRentalsAsync();
    }
}
            
        
    
