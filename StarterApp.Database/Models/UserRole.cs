using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarterApp.Database.Models;

/// <summary>
/// Represents the join entity between users and roles.
/// This allows each user to be assigned one or more authorization roles.
/// </summary>
[Table("user_role")]
[PrimaryKey(nameof(Id))]
public class UserRole
{
    /// <summary>
    /// Unique user-role assignment identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifier of the user assigned to the role.
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Identifier of the assigned role.
    /// </summary>
    [Required]
    public int RoleId { get; set; }

    /// <summary>
    /// User connected to this role assignment.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    /// <summary>
    /// Role connected to this user assignment.
    /// </summary>
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = null!;

    /// <summary>
    /// Date and time when the assignment was created.
    /// </summary>
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the assignment was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the assignment was soft-deleted, if applicable.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Indicates whether the user-role assignment is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Creates an active user-role assignment with default timestamps.
    /// </summary>
    public UserRole()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Creates an active user-role assignment for a specific user and role.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="roleId">The role identifier.</param>
    public UserRole(int userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    /// <summary>
    /// Updates the assignment's UpdatedAt timestamp to the current UTC time.
    /// </summary>
    public void UpdateTimestamps()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Soft-deletes the assignment and marks it as inactive.
    /// </summary>
    public void MarkAsDeleted()
    {
        DeletedAt = DateTime.UtcNow;
        IsActive = false;
    }

    /// <summary>
    /// Restores a soft-deleted assignment and marks it as active.
    /// </summary>
    public void Restore()
    {
        DeletedAt = null;
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Returns a string representation of the user-role assignment.
    /// </summary>
    /// <returns>A readable summary of the assignment.</returns>
    public override string ToString()
    {
        return $"UserRole(Id: {Id}, UserId: {UserId}, RoleId: {RoleId}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}, DeletedAt: {DeletedAt}, IsActive: {IsActive})";
    }

    /// <summary>
    /// Compares this user-role assignment with another object for equality.
    /// </summary>
    /// <param name="obj">The object to compare against.</param>
    /// <returns>True if the object represents the same assignment values; otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is UserRole other)
        {
            return Id == other.Id &&
                   UserId == other.UserId &&
                   RoleId == other.RoleId &&
                   CreatedAt == other.CreatedAt &&
                   UpdatedAt == other.UpdatedAt &&
                   DeletedAt == other.DeletedAt &&
                   IsActive == other.IsActive;
        }

        return false;
    }

    /// <summary>
    /// Generates a hash code for the user-role assignment.
    /// </summary>
    /// <returns>A hash code based on the assignment values.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, UserId, RoleId, CreatedAt, UpdatedAt, DeletedAt, IsActive);
    }
}
