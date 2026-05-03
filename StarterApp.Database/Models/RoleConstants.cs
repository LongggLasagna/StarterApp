namespace StarterApp.Database.Models;

/// <summary>
/// Contains the role names used throughout the application for authorization checks.
/// Keeping role names in one place avoids hard-coded strings across the codebase.
/// </summary>
public static class RoleConstants
{
    /// <summary>
    /// Role name for administrators with elevated permissions.
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// Role name for standard application users.
    /// </summary>
    public const string OrdinaryUser = "OrdinaryUser";

    /// <summary>
    /// Role name for users with additional non-admin permissions.
    /// </summary>
    public const string SpecialUser = "SpecialUser";

    /// <summary>
    /// Collection of all supported role names.
    /// </summary>
    public static readonly string[] AllRoles = { Admin, OrdinaryUser, SpecialUser };
}