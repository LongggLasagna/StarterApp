using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public class ItemsListViewModel : BaseViewModel
{
    public ObservableCollection<string> Items { get; } = new();

    public ItemsListViewModel()
    {
        Title = "Items List";

        Items.Add("Item 1");
        Items.Add("Item 2");
        Items.Add("Item 3");

    }
}