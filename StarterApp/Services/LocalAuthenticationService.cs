using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;
using BCrypt.Net;

namespace StarterApp.Services;

/// <summary>
/// Provides local database authentication using Entity Framework Core and BCrypt password hashing.
/// This service is used when the app is configured to authenticate against the local database instead of the shared API.
/// </summary>
public class LocalAuthenticationService : IAuthenticationService
{
    private readonly AppDbContext _context;
    private User? _currentUser;
    private List<string> _currentUserRoles = new();

    /// <summary>
    /// Raised when the user's authentication state changes.
    /// </summary>
    public event EventHandler<bool>? AuthenticationStateChanged;

    /// <summary>
    /// Creates a local authentication service using the application database context.
    /// </summary>
    /// <param name="context">The Entity Framework database context.</param>
    public LocalAuthenticationService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Indicates whether a user is currently authenticated.
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
    /// Authenticates a user against the local database using email and password.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's plain-text password.</param>
    /// <returns>An authentication result describing success or failure.</returns>
    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

            if (user == null)
            {
                return new AuthenticationResult(false, "Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new AuthenticationResult(false, "Invalid email or password");
            }

            _currentUser = user;
            _currentUserRoles = user.UserRoles
                .Where(ur => ur.IsActive)
                .Select(ur => ur.Role.Name)
                .ToList();

            AuthenticationStateChanged?.Invoke(this, true);
            return new AuthenticationResult(true, "Login successful");
        }
        catch (Exception ex)
        {
            return new AuthenticationResult(false, $"Login failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Registers a new user in the local database and assigns the default role.
    /// </summary>
    /// <param name="firstName">The user's first name.</param>
    /// <param name="lastName">The user's last name.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's plain-text password.</param>
    /// <returns>An authentication result describing success or failure.</returns>
    public async Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return new AuthenticationResult(false, "User with this email already exists");
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.IsDefault == true);
            if (userRole != null)
            {
                var userRoleAssignment = new UserRole(user.Id, userRole.Id);
                _context.UserRoles.Add(userRoleAssignment);
                await _context.SaveChangesAsync();
            }

            return new AuthenticationResult(true, "Registration successful");
        }
        catch (Exception ex)
        {
            return new AuthenticationResult(false, $"Registration failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Logs out the current user and clears local authentication state.
    /// </summary>
    public Task LogoutAsync()
    {
        _currentUser = null;
        _currentUserRoles.Clear();
        AuthenticationStateChanged?.Invoke(this, false);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Checks whether the current user has a specific role.
    /// </summary>
    /// <param name="roleName">The role name to check.</param>
    /// <returns>True if the current user has the role; otherwise false.</returns>
    public bool HasRole(string roleName)
    {
        return _currentUserRoles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks whether the current user has at least one of the provided roles.
    /// </summary>
    /// <param name="roleNames">The role names to check.</param>
    /// <returns>True if the user has any supplied role; otherwise false.</returns>
    public bool HasAnyRole(params string[] roleNames)
    {
        return roleNames.Any(role => HasRole(role));
    }

    /// <summary>
    /// Checks whether the current user has all provided roles.
    /// </summary>
    /// <param name="roleNames">The role names to check.</param>
    /// <returns>True if the user has all supplied roles; otherwise false.</returns>
    public bool HasAllRoles(params string[] roleNames)
    {
        return roleNames.All(role => HasRole(role));
    }

    /// <summary>
    /// Changes the password for the currently authenticated local user.
    /// The current password must match before the new password is stored.
    /// </summary>
    /// <param name="currentPassword">The user's current password.</param>
    /// <param name="newPassword">The user's new password.</param>
    /// <returns>True if the password was changed successfully; otherwise false.</returns>
    public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        if (_currentUser == null)
            return false;

        try
        {
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, _currentUser.PasswordHash))
            {
                return false;
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, salt);

            _currentUser.PasswordHash = hashedPassword;
            _currentUser.PasswordSalt = salt;
            _currentUser.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(_currentUser);
            await _context.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}

/// <summary>
/// Represents the result of an authentication operation.
/// </summary>
public class AuthenticationResult
{
    /// <summary>
    /// Indicates whether the authentication operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Message describing the result of the authentication operation.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Creates a new authentication result.
    /// </summary>
    /// <param name="isSuccess">Whether the operation succeeded.</param>
    /// <param name="message">The result message.</param>
    public AuthenticationResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }
}