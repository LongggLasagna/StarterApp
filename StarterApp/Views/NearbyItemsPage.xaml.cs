using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class NearbyItemsPage : ContentPage
{
    public NearbyItemsPage(NearbyItemsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is NearbyItemsViewModel vm)
        {
            await vm.LoadNearbyItemsAsync();
        }
    }

}
