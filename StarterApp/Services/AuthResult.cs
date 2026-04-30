using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Represents the result of an authentication operation.
/// Contains success status, error information, the authenticated user, and assigned roles.
/// </summary>
public class AuthResult
{
    /// <summary>
    /// Indicates whether the authentication operation succeeded.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error message returned when authentication fails.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// The authenticated user when authentication succeeds.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Roles assigned to the authenticated user.
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Creates a successful authentication result.
    /// </summary>
    /// <param name="user">The authenticated user.</param>
    /// <param name="roles">The roles assigned to the user.</param>
    /// <returns>A successful authentication result.</returns>
    public static AuthResult Success(User user, List<string> roles)
    {
        return new AuthResult
        {
            IsSuccess = true,
            User = user,
            Roles = roles
        };
    }

    /// <summary>
    /// Creates a failed authentication result.
    /// </summary>
    /// <param name="errorMessage">The reason authentication failed.</param>
    /// <returns>A failed authentication result.</returns>
    public static AuthResult Failure(string errorMessage)
    {
        return new AuthResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}