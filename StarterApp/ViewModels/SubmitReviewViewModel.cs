using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class SubmitReviewViewModel : BaseViewModel
{
    private readonly IReviewService _reviewService;
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;

    private Item? item;

    [ObservableProperty]
    private int rating = 5;

    [ObservableProperty]
    private string comment = string.Empty;

    public SubmitReviewViewModel(
        IReviewService reviewService,
        IAuthenticationService authService,
        INavigationService navigationService)
    {
        _reviewService = reviewService;
        _authService = authService;
        _navigationService = navigationService;

        Title = "Submit Review";
    }

    public void LoadItem(Item selectedItem)
    {
        item = selectedItem;
    }

    [RelayCommand]
    private async Task SubmitAsync()
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

            await _reviewService.SubmitReviewAsync(item, user, Rating, Comment);

            await Application.Current.MainPage.DisplayAlert(
                "Success",
                "Review submitted",
                "OK");

            await _navigationService.NavigateBackAsync();
        }
        catch (Exception ex)
        {
            SetError(ex.Message);
        }
    }
}
