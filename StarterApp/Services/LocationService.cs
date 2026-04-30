namespace StarterApp.Services;

/// <summary>
/// Provides access to the device GPS location for nearby item discovery.
/// </summary>
public class LocationService : ILocationService
{
    /// <summary>
    /// Attempts to retrieve the user's current device location.
    /// Returns null if permission is denied or the location cannot be retrieved.
    /// </summary>
    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(
                GeolocationAccuracy.Medium,
                TimeSpan.FromSeconds(10));

            return await Geolocation.Default.GetLocationAsync(request);
        }
        catch
        {
            return null;
        }
    }
}
