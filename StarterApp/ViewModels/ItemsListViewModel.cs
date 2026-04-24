using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using StarterApp.Database.Data.Repositories;
using StarterApp.Views;

namespace StarterApp.ViewModels;

public partial class ItemsListViewModel : BaseViewModel
{

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
    private readonly IItemRepository _itemRepository;
    public ObservableCollection<Item> Items { get; } = new();

    public ItemsListViewModel(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
        Title = "Items List";
    }

    public async Task LoadItemsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Items.Clear();

            var items = await _itemRepository.GetAllAsync();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load items: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

   
}