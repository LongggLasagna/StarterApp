using StarterApp.Database.Models;
using StarterApp.Services;
using StarterApp.ViewModels;

namespace StarterApp.Views;

[QueryProperty(nameof(Item), "Item")]
public partial class EditItemPage : ContentPage
{
    public Item Item
    {
        set
        {
            if (BindingContext is EditItemViewModel viewModel)
            {
                viewModel.LoadItem(value);
            }
        }
    }

    public EditItemPage(EditItemViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}