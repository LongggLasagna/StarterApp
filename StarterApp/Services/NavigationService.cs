namespace StarterApp.Services;

/// <summary>
/// Provides Shell-based navigation for the application.
/// This service lets ViewModels request navigation without directly depending on page code.
/// </summary>
public class NavigationService : INavigationService
{
    /// <summary>
    /// Navigates to a registered Shell route.
    /// </summary>
    /// <param name="route">The route to navigate to.</param>
    public async Task NavigateToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    /// <summary>
    /// Navigates to a registered Shell route and passes parameters to the destination page.
    /// </summary>
    /// <param name="route">The route to navigate to.</param>
    /// <param name="parameters">The parameters to pass to the destination page.</param>
    public async Task NavigateToAsync(string route, Dictionary<string, object> parameters)
    {
        await Shell.Current.GoToAsync(route, parameters);
    }

    /// <summary>
    /// Navigates back one page in the current navigation stack.
    /// </summary>
    public async Task NavigateBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    /// <summary>
    /// Navigates back to the login/root route.
    /// </summary>
    public async Task NavigateToRootAsync()
    {
        await Shell.Current.GoToAsync("//login");
    }

    /// <summary>
    /// Removes pages from the navigation stack until the root page is reached.
    /// </summary>
    public async Task PopToRootAsync()
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }
}