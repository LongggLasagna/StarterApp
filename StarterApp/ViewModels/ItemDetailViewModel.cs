using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Views;

namespace StarterApp.ViewModels;

public partial class ItemDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    private Item? item;

    public ItemDetailViewModel()
    {
        Title = "Item Details";
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