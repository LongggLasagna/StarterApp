using StarterApp.ViewModels;
using StarterApp.Views;


namespace StarterApp;

public partial class AppShell : Shell
{
	public AppShell(AppShellViewModel viewModel)
	{	
		BindingContext = viewModel;
		InitializeComponent();

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(UserListPage), typeof(UserListPage));
		Routing.RegisterRoute(nameof(TempPage), typeof(TempPage));
		Routing.RegisterRoute(nameof(ItemsListPage), typeof(ItemsListPage));

	}
}
