using System.Net.Http.Headers;
using System.Net.Http.Json;
using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Provides authentication against the shared backend API.
/// Handles login, registration, logout, current user state, and JWT authorization headers.
/// </summary>
public class ApiAuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private User? _currentUser;
    private readonly List<string> _currentUserRoles = new();

    /// <summary>
    /// Raised whenever the authentication state changes.
    /// The boolean value indicates whether the user is currently authenticated.
    /// </summary>
    public event EventHandler<bool>? AuthenticationStateChanged;

    /// <summary>
    /// Indicates whether a user is currently logged in.
    /// </summary>
    public bool IsAuthenticated => _currentUser != null;

    /// <summary>
    /// The currently authenticated user, or null when no user is logged in.
    /// </summary>
    public User? CurrentUser => _currentUser;

    /// <summary>
    /// Roles assigned to the currently authenticated user.
    /// </summary>
    public List<string> CurrentUserRoles => _currentUserRoles;

    /// <summary>
    /// Creates a new API authentication service using the configured HTTP client.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to communicate with the backend API.</param>
    public ApiAuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Authenticates a user using their email and password.
    /// On success, stores the JWT token in the HTTP authorization header and loads the user profile.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>An authentication result indicating success or failure.</returns>
    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/token", new { email, password });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return new AuthenticationResult(false, error?.Message ?? "Login failed");
            }

            var token = await response.Content.ReadFromJsonAsync<TokenResponse>();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token!.Token);

            var meResponse = await _httpClient.GetAsync("users/me");
            var profile = await meResponse.Content.ReadFromJsonAsync<UserProfileResponse>();

            _currentUser = new User
            {
                Id = profile!.Id,
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                CreatedAt = profile.CreatedAt,
                IsActive = true
            };

            AuthenticationStateChanged?.Invoke(this, true);
            return new AuthenticationResult(true, "Login successful");
        }
        catch (Exception ex)
        {
            return new AuthenticationResult(false, $"Login failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Registers a new user account through the backend API.
    /// </summary>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's chosen password.</param>
    /// <returns>An authentication result indicating whether registration succeeded.</returns>
    public async Task<AuthenticationResult> RegisterAsync(
        string firstName,
        string lastName,
        string email,
        string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", new
            {
                firstName,
                lastName,
                email,
                password
            });

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return new AuthenticationResult(false, $"Registration failed: {body}");
            }

            return new AuthenticationResult(true, "Registration successful. Please log in.");
        }
        catch (Exception ex)
        {
            return new AuthenticationResult(false, $"Registration failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Logs the current user out by clearing the current user, roles, and JWT authorization header.
    /// </summary>
    /// <returns>A completed task.</returns>
    public Task LogoutAsync()
    {
        _currentUser = null;
        _currentUserRoles.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = null;
        AuthenticationStateChanged?.Invoke(this, false);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Checks whether the current user has a specific role.
    /// </summary>
    /// <param name="roleName">The role name to check.</param>
    /// <returns>True if the user has the role; otherwise false.</returns>
    public bool HasRole(string roleName) =>
        _currentUserRoles.Contains(roleName, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Checks whether the current user has at least one of the specified roles.
    /// </summary>
    /// <param name="roleNames">Role names to check.</param>
    /// <returns>True if the user has any supplied role; otherwise false.</returns>
    public bool HasAnyRole(params string[] roleNames) =>
        roleNames.Any(HasRole);

    /// <summary>
    /// Checks whether the current user has all of the specified roles.
    /// </summary>
    /// <param name="roleNames">Role names to check.</param>
    /// <returns>True if the user has every supplied role; otherwise false.</returns>
    public bool HasAllRoles(params string[] roleNames) =>
        roleNames.All(HasRole);

    /// <summary>
    /// Attempts to change the user's password.
    /// This is currently unsupported by the shared backend API.
    /// </summary>
    /// <param name="currentPassword">The user's current password.</param>
    /// <param name="newPassword">The user's new password.</param>
    /// <returns>False because password changes are not supported by the API.</returns>
    public Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        return Task.FromResult(false);
    }

    /// <summary>
    /// Represents the JWT token response returned by the authentication endpoint.
    /// </summary>
    private record TokenResponse(string Token, DateTime ExpiresAt, int UserId);

    /// <summary>
    /// Represents the current user's profile returned by the backend API.
    /// </summary>
    private record UserProfileResponse(
        int Id,
        string Email,
        string FirstName,
        string LastName,
        DateTime CreatedAt);

    /// <summary>
    /// Represents an error response returned by the backend API.
    /// </summary>
    private record ApiErrorResponse(string Error, string Message);
}