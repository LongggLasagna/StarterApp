using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class EditItemViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly INavigationService _navigationService;

    private Item? item;

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

    public EditItemViewModel(
        IItemRepository itemRepository,
        IAuthenticationService authenticationService,
        INavigationService navigationService)
    {
        _itemRepository = itemRepository;
        _authenticationService = authenticationService;
        _navigationService = navigationService;
        Title = "Edit Item";
    }

    public void LoadItem(Item selectedItem)
    {
        item = selectedItem;

        TitleText = selectedItem.Title;
        DescriptionText = selectedItem.Description;
        CategoryText = selectedItem.Category;
        LocationText = selectedItem.Location;
        DailyRateText = selectedItem.DailyRate.ToString("F2");
    }

    [RelayCommand]
    private async Task SaveItemAsync()
    {
        try
        {
            ClearError();
            
            if (item == null)
            {
                SetError("No item loaded to save.");
                return;
            }

            var currentUser = _authenticationService.CurrentUser;
            if (currentUser == null || currentUser.Id != item.OwnerId)
            {
                SetError("You can only edit items that you own.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TitleText))
            {
                SetError("Title is required.");
                return;
            }
            if (!decimal.TryParse(DailyRateText, out var dailyRate) || dailyRate < 0)
            {
                SetError("Daily Rate must be a valid non-negative number.");
                return;
            }

            item.Title = TitleText.Trim();
            item.Description = DescriptionText.Trim();
            item.Category = CategoryText.Trim();
            item.Location = LocationText.Trim();
            item.DailyRate = dailyRate;

            await _itemRepository.UpdateAsync(item);

            await _navigationService.NavigateBackAsync();
            }
            catch (Exception ex)
            {
                SetError($"Failed to save item: {ex.Message}");
            }
    }
}