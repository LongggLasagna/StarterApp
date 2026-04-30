using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarterApp.Database.Models;

/// <summary>
/// Represents an application role used for authorization and access control.
/// Roles can be assigned to users through the UserRole join entity.
/// </summary>
[Table("role")]
[PrimaryKey(nameof(Id))]
public class Role
{
    /// <summary>
    /// Unique role identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Role name, such as User or Admin.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of what the role is used for.
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether this role should be assigned to new users by default.
    /// </summary>
    public bool IsDefault { get; set; } = false;

    /// <summary>
    /// User-role assignments linked to this role.
    /// </summary>
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
}