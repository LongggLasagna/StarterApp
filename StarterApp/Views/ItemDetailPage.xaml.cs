using StarterApp.Database.Models;
using StarterApp.ViewModels;

namespace StarterApp.Views;

[QueryProperty(nameof(Item), "Item")]
public partial class ItemDetailPage : ContentPage
{
    private readonly ItemDetailViewModel _viewModel;
    private Item? item;

    public Item? Item
    {
        get => item;
        set
        {
            item = value;

            if (value != null)
            {
                _viewModel.Item = value;
            }
        }
    }

    public ItemDetailPage(ItemDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadReviewsAsync();
    }
}