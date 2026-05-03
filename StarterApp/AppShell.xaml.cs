using StarterApp.ViewModels;
using StarterApp.Views;

namespace StarterApp;

/// <summary>
/// Defines the application's Shell navigation structure and registers named routes.
/// The Shell is used to navigate between MAUI pages such as items, rentals, reviews, and nearby search.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Creates the application shell, applies the Shell ViewModel, and registers application routes.
    /// </summary>
    /// <param name="viewModel">The ViewModel used by the Shell for navigation-related commands.</param>
    public AppShell(AppShellViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(UserListPage), typeof(UserListPage));
        Routing.RegisterRoute(nameof(TempPage), typeof(TempPage));
        Routing.RegisterRoute(nameof(ItemsListPage), typeof(ItemsListPage));
        Routing.RegisterRoute(nameof(CreateItemPage), typeof(CreateItemPage));
        Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        Routing.RegisterRoute(nameof(EditItemPage), typeof(EditItemPage));
        Routing.RegisterRoute(nameof(RequestRentalPage), typeof(RequestRentalPage));
        Routing.RegisterRoute(nameof(RentalsPage), typeof(RentalsPage));
        Routing.RegisterRoute(nameof(SubmitReviewPage), typeof(SubmitReviewPage));
        Routing.RegisterRoute(nameof(NearbyItemsPage), typeof(NearbyItemsPage));
    }
}
