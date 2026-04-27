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
    private readonly IReviewService _reviewService;
    public ObservableCollection<Review> Reviews { get; } = new();
    [ObservableProperty]
    private double averageRating;

     [ObservableProperty]
     private Item? item;

    public bool CanEditItem => Item != null && 
    _authenticationService.CurrentUser != null &&
    Item.OwnerId == _authenticationService.CurrentUser.Id;

    public ItemDetailViewModel(IAuthenticationService authenticationService, IReviewService reviewService)
    {
        _authenticationService = authenticationService;
        _reviewService = reviewService;
        Title = "Item Details";
    }

    partial void OnItemChanged(Item? oldValue, Item? newValue)
    {
        OnPropertyChanged(nameof(CanEditItem));
        _ = LoadReviewsAsync();
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

            Reviews.Clear();

            var reviews = await _reviewService.GetReviewsForItemAsync(Item.Id);

            foreach (var review in reviews)
            {
                Reviews.Add(review);
            }
            AverageRating = await _reviewService.GetAverageRatingForItemAsync(Item.Id);
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