namespace StarterApp.Services;

public interface ILocationService
{
    Task<Location?> GetCurrentLocationAsync();
}

