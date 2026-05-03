using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Provides data for authentication state change events.
/// Includes whether the user is authenticated, the current user, and their roles.
/// </summary>
public class AuthStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Indicates whether the application currently has an authenticated user.
    /// </summary>
    public bool IsAuthenticated { get; set; }

    /// <summary>
    /// The authenticated user, or null when the user has logged out.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// The roles assigned to the authenticated user.
    /// </summary>
    public List<string> Roles { get; set; } = new();
}