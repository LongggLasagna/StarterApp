using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarterApp.Database.Models;

/// <summary>
/// Represents an application user account.
/// Users can authenticate, own item listings, request rentals, and submit reviews.
/// </summary>
[Table("users")]
[PrimaryKey(nameof(Id))]
public class User
{
    /// <summary>
    /// Unique user identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User's first name.
    /// </summary>
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name.
    /// </summary>
    [Required]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address used for login.
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password stored for local authentication.
    /// </summary>
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Salt used when hashing the user's password.
    /// </summary>
    [Required]
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the user account was created.
    /// </summary>
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the user account was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the user account was soft-deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Indicates whether the user account is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Role assignments connected to this user.
    /// </summary>
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// Combined first and last name for display purposes.
    /// </summary>
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}