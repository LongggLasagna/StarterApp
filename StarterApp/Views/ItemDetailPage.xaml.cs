using StarterApp.Database.Models;
using StarterApp.ViewModels;

namespace StarterApp.Views;

[QueryProperty(nameof(Item), "Item")]
public partial class ItemDetailPage : ContentPage
{
    public Item Item
    {
        set
        {
            if (BindingContext is ItemDetailViewModel vm)
            {
                vm.Item = value;
            }
        }
    }

    public ItemDetailPage(ItemDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

