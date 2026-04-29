using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class SubmitReviewViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly INavigationService _navigationService;

    private int rentalId;

    [ObservableProperty]
    private int rating = 5;

    [ObservableProperty]
    private string comment = string.Empty;

    public SubmitReviewViewModel(
        IApiService apiService,
        INavigationService navigationService)
    {
        _apiService = apiService;
        _navigationService = navigationService;
        Title = "Submit Review";
    }

    public void LoadRental(int selectedRentalId)
    {
        rentalId = selectedRentalId;
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        try
        {
            ClearError();

            if (rentalId <= 0)
            {
                SetError("No rental selected.");
                return;
            }

            await _apiService.CreateReviewAsync(rentalId, Rating, Comment);

            await Application.Current.MainPage.DisplayAlert(
                "Success",
                "Review submitted.",
                "OK");

            await _navigationService.NavigateBackAsync();
        }
        catch (Exception ex)
        {
            SetError(ex.Message);
        }
    }
}