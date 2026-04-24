using StarterApp.Database.Models;
using StarterApp.ViewModels;

namespace StarterApp.Views;

[QueryProperty(nameof(Item), "Item")]
public partial class RequestRentalPage : ContentPage
{
    public Item Item
    {
        set
        {
            if (BindingContext is RequestRentalViewModel vm)
            {
                vm.LoadItem(value);
            }
        }
    }

    public RequestRentalPage(RequestRentalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
