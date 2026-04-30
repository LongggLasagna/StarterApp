using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class RequestRentalViewModel : BaseViewModel
{
    private readonly IRentalService _rentalService;
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
        IRentalService rentalService,
        IAuthenticationService authService,
        INavigationService navigationService)
    {
        _rentalService = rentalService;
        _authService = authService;
        _navigationService = navigationService;

        Title = "Rental Request";
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
        if (item == null)
            return;

        TotalPrice = _rentalService.CalculateTotalPrice(item, StartDate, EndDate);
    }

    [RelayCommand]
    private async Task SubmitRequestAsync()
    {
        try
        {
            ClearError();

            if (item == null)
            {
                SetError("No item selected.");
                return;
            }

            var user = _authService.CurrentUser;
            if (user == null)
            {
                SetError("You must be logged in.");
                return;
            }

            await _rentalService.RequestRentalAsync(item, user, StartDate, EndDate);

            await Application.Current.MainPage.DisplayAlert(
                "Success",
                "Rental request submitted.",
                "OK");

            await _navigationService.NavigateBackAsync();
        }
        catch (Exception ex)
        {
            var message = ex.InnerException?.Message ?? ex.Message;
            SetError($"Failed to request rental: {message}");
        }
    }
}