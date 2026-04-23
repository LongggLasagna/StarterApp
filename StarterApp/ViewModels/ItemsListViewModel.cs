using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;
using StarterApp.Database.Data.Repositories;

namespace StarterApp.ViewModels;

public class ItemsListViewModel : BaseViewModel
{
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