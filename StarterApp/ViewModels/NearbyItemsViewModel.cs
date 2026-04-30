using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using StarterApp.Views;

namespace StarterApp.ViewModels;

public partial class NearbyItemsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly ILocationService _locationService;

    public ObservableCollection<Item> Items { get; } = new();

    [ObservableProperty]
    private double radiusKm = 5;

    public NearbyItemsViewModel(
        IApiService apiService,
        ILocationService locationService)
    {
        _apiService = apiService;
        _locationService = locationService;
        Title = "Find Near Me";
    }

    [RelayCommand]
    public async Task LoadNearbyItemsAsync()
    {
        try
        {
            ClearError();
            Items.Clear();

            var location = await _locationService.GetCurrentLocationAsync();

            if (location == null)
            {
                SetError("Could not get your location.");
                return;
            }

            var nearbyItems = await _apiService.GetNearbyItemsAsync(
                location.Latitude,
                location.Longitude,
                RadiusKm);

            foreach (var item in nearbyItems)
            {
                Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            SetError(ex.Message);
        }
    }

    [RelayCommand]
    private async Task ItemSelectedAsync(Item item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync(nameof(ItemDetailPage), new Dictionary<string, object>
        {
            { "Item", item }
        });
    }
}