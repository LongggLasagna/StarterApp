namespace StarterApp.Services;

/// <summary>
/// Defines location operations used by the application.
/// This abstraction keeps GPS access separate from ViewModels.
/// </summary>
public interface ILocationService
{
     /// <summary>
    /// Attempts to retrieve the current device location.
    /// </summary>
    /// <returns>
    /// The current location if available; otherwise null if permission is denied
    /// or the device cannot provide a location.
    /// </returns>
    Task<Location?> GetCurrentLocationAsync();
}

