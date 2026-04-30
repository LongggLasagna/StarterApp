using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using StarterApp.Views;

namespace StarterApp.ViewModels;

public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IApiService _apiService;
    public ObservableCollection<Review> Reviews { get; } = new();
    [ObservableProperty]
    private double averageRating;

     [ObservableProperty]
     private Item? item;

    public bool CanEditItem => Item != null && 
    _authenticationService.CurrentUser != null &&
    Item.OwnerId == _authenticationService.CurrentUser.Id;

    public ItemDetailViewModel(IAuthenticationService authenticationService, IApiService apiService)
    {
        
        _authenticationService = authenticationService;
        _apiService = apiService;
        Title = "Item Details";
    }

   partial void OnItemChanged(Item? value)
    {
        OnPropertyChanged(nameof(CanEditItem));
    }
    
    [RelayCommand]
    private async Task EditItemAsync()
    {
        if (Item == null)
            return;

        await Shell.Current.GoToAsync(nameof(EditItemPage), new Dictionary<string, object>
        {
            { "Item", Item }
        });
    }
    
    [RelayCommand]
    private async Task RequestRentalAsync()
    {
        if (Item == null)
        return;

        await Shell.Current.GoToAsync(nameof(RequestRentalPage), new Dictionary<string, object>
        {
            {"Item", Item}
        });
    }

    public async Task LoadReviewsAsync()
        {
            if (Item == null) return;

            try
            {
                Reviews.Clear();

                var reviews = await _apiService.GetItemReviewsAsync(Item.Id);

                foreach (var review in reviews)
                {
                    Reviews.Add(review);
                    AverageRating = reviews.Any()
                    ? reviews.Average(r => r.Rating)
                    : 0;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }


    [RelayCommand]
    private async Task AddReviewAsync()
    {
        if (Item == null)
            return;

        await Shell.Current.GoToAsync(nameof(SubmitReviewPage), new Dictionary<string, object>
        {
            { "Item", Item }
        });
    }
    
}