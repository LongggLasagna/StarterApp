namespace StarterApp.Services;

/// <summary>
/// Defines navigation operations used by ViewModels.
/// This abstraction keeps Shell navigation separate from ViewModel logic.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to a route registered in the MAUI Shell.
    /// </summary>
    /// <param name="route">The Shell route to navigate to.</param>
    Task NavigateToAsync(string route);

    /// <summary>
    /// Navigates to a route and passes query parameters to the target page.
    /// </summary>
    /// <param name="route">The Shell route to navigate to.</param>
    /// <param name="parameters">The parameters passed to the target route.</param>
    Task NavigateToAsync(string route, Dictionary<string, object> parameters);

    /// <summary>
    /// Navigates back one page in the navigation stack.
    /// </summary>
    Task NavigateBackAsync();

    /// <summary>
    /// Navigates back to the application root route.
    /// </summary>
    Task NavigateToRootAsync();

    /// <summary>
    /// Pops all pages from the navigation stack and returns to the root page.
    /// </summary>
    Task PopToRootAsync();
}