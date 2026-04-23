using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class CreateItemViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string titleText = string.Empty;

    [ObservableProperty]
    private string descriptionText = string.Empty;

    [ObservableProperty]
    private string categoryText = string.Empty;

    [ObservableProperty]
    private string locationText = string.Empty;

    [ObservableProperty]
    private string dailyRateText = string.Empty;

    public CreateItemViewModel(
        IItemRepository itemRepository,
        IAuthenticationService authenticationService,
        INavigationService navigationService)
    {
        _itemRepository = itemRepository;
        _authenticationService = authenticationService;
        _navigationService = navigationService;
        Title = "Create Item";
    }

    [RelayCommand]
    private async Task CreateItemAsync()
    {
        try
        {
            ClearError();

            if (string.IsNullOrWhiteSpace(TitleText))
            {
                SetError("Title is required.");
                return;
            }

            if (!decimal.TryParse(DailyRateText, out var dailyRate) || dailyRate <= 0)
            {
                SetError("Daily rate must be a valid number greater than zero.");
                return;
            }

            var currentUser = _authenticationService.CurrentUser;
            if (currentUser == null)
            {
                SetError("You must be logged in to create an item.");
                return;
            }

            var item = new Item
            {
                Title = TitleText.Trim(),
                Description = DescriptionText.Trim(),
                Category = CategoryText.Trim(),
                Location = LocationText.Trim(),
                DailyRate = dailyRate,
                OwnerId = currentUser.Id
            };

            await _itemRepository.AddAsync(item);

            await Application.Current.MainPage.DisplayAlert(
                "Success",
                "Item created successfully.",
                "OK");

            await _navigationService.NavigateBackAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to create item: {ex.Message}");
        }
    }
}
