using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Defines authentication operations and current authentication state for the application.
/// Implementations may authenticate against local storage or the shared backend API.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Raised when the user's authentication state changes.
    /// The boolean value indicates whether the user is authenticated.
    /// </summary>
    event EventHandler<bool>? AuthenticationStateChanged;

    /// <summary>
    /// Indicates whether a user is currently authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the currently authenticated user, or null when no user is logged in.
    /// </summary>
    User? CurrentUser { get; }

    /// <summary>
    /// Gets the roles assigned to the currently authenticated user.
    /// </summary>
    List<string> CurrentUserRoles { get; }

    /// <summary>
    /// Authenticates a user using email and password credentials.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>An authentication result describing success or failure.</returns>
    Task<AuthenticationResult> LoginAsync(string email, string password);

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>An authentication result describing success or failure.</returns>
    Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password);

    /// <summary>
    /// Logs out the current user and clears authentication state.
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    /// Checks whether the current user has a specific role.
    /// </summary>
    /// <param name="roleName">The role name to check.</param>
    /// <returns>True if the user has the role; otherwise false.</returns>
    bool HasRole(string roleName);

    /// <summary>
    /// Checks whether the current user has at least one of the provided roles.
    /// </summary>
    /// <param name="roleNames">The role names to check.</param>
    /// <returns>True if the user has any of the roles; otherwise false.</returns>
    bool HasAnyRole(params string[] roleNames);

    /// <summary>
    /// Checks whether the current user has all provided roles.
    /// </summary>
    /// <param name="roleNames">The role names to check.</param>
    /// <returns>True if the user has all roles; otherwise false.</returns>
    bool HasAllRoles(params string[] roleNames);

    /// <summary>
    /// Changes the current user's password where supported by the authentication provider.
    /// </summary>
    /// <param name="currentPassword">The current password.</param>
    /// <param name="newPassword">The new password.</param>
    /// <returns>True if the password was changed successfully; otherwise false.</returns>
    Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
}