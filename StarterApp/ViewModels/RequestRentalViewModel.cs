using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;


namespace StarterApp.ViewModels;

public partial class RequestRentalViewModel : BaseViewModel
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;

    private Item? item;

    [ObservableProperty]
    private DateTime startDate = DateTime.Today;

    [ObservableProperty]
    private DateTime endDate = DateTime.Today.AddDays(1);

    [ObservableProperty]
    private decimal totalPrice; 

    public RequestRentalViewModel(
        IRentalRepository rentalRepository,
        IAuthenticationService authService,
        INavigationService navigationService)
    {
        _rentalRepository = rentalRepository;
        _authService = authService;
        _navigationService = navigationService;

        Title = "RENTAL REQUEST";
    }

    public void LoadItem(Item selectedItem)
    {
        item = selectedItem;
        CalculatePrice();
    }

    partial void OnStartDateChanged(DateTime value) => CalculatePrice();
    partial void OnEndDateChanged(DateTime value) => CalculatePrice();

    private void CalculatePrice()
    {
        if (item == null) return;
        {
            var days = (EndDate - StartDate).Days;
            if (days <= 0) days = 1;

            TotalPrice = days * item.DailyRate;
        }
    }
        [RelayCommand]
        private async Task SubmitRequestAsync()
        {
            try
            {
                ClearError();

                if (item == null)
                {
                    SetError("NoItemSelected");
                    return;
                }

                var user = _authService.CurrentUser;
                if (user == null)
                {
                    SetError("You msut be logged in");
                    return;
                }

                var rental = new Rental
                {
                    ItemId =item.Id,
                    BorrowerId = user.Id,
                    StartDate = DateTime.SpecifyKind(StartDate.Date, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(EndDate.Date, DateTimeKind.Utc),
                    TotalPrice = TotalPrice,
                    Status = RentalStatus.Requested
                };

                await _rentalRepository.AddAsync(rental);

                await Application.Current.MainPage.DisplayAlert(
                    "Success",
                    "Rental request Submitted",
                    "Okay");

                await _navigationService.NavigateBackAsync();
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                SetError($"failed to request rental: {ex.Message}");
            }
        }
    
    
}