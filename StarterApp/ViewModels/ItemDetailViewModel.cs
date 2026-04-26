using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Views;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;

    [ObservableProperty]
    private Item? item;

    public bool CanEditItem => Item != null && 
    _authenticationService.CurrentUser != null &&
    Item.OwnerId == _authenticationService.CurrentUser.Id;

    public ItemDetailViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        Title = "Item Details";
    }

    partial void OnItemChanged(Item? oldValue, Item? newValue)
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

    
}