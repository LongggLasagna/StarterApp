using StarterApp.Database.Models;
using Xunit;

namespace StarterApp.Tests.Services;

public class UserRoleTests
{
    [Fact]
    public void MarkAsDeleted_WhenCalled_SetsDeletedAtAndDeactivates()
    {
        // Arrange
        var userRole = new UserRole(1, 2);

        // Act
        userRole.MarkAsDeleted();

        // Assert
        Assert.NotNull(userRole.DeletedAt);
        Assert.False(userRole.IsActive);
    }

    [Fact]
    public void Restore_WhenCalled_ClearsDeletedAtAndActivates()
    {
        // Arrange
        var userRole = new UserRole(1, 2);
        userRole.MarkAsDeleted();

        // Act
        userRole.Restore();

        // Assert
        Assert.Null(userRole.DeletedAt);
        Assert.True(userRole.IsActive);
    }

    [Fact]
    public void Equals_WhenValuesMatch_ReturnsTrue()
    {
        // Arrange
        var createdAt = new DateTime(2026, 1, 1);
        var updatedAt = new DateTime(2026, 1, 2);

        var first = new UserRole
        {
            Id = 1,
            UserId = 2,
            RoleId = 3,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = null,
            IsActive = true
        };

        var second = new UserRole
        {
            Id = 1,
            UserId = 2,
            RoleId = 3,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = null,
            IsActive = true
        };

        // Act
        var result = first.Equals(second);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenObjectIsDifferentType_ReturnsFalse()
    {
        // Arrange
        var userRole = new UserRole(1, 2);

        // Act
        var result = userRole.Equals("not a user role");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ToString_WhenCalled_ReturnsReadableText()
    {
        // Arrange
        var userRole = new UserRole(1, 2)
        {
            Id = 10
        };

        // Act
        var result = userRole.ToString();

        // Assert
        Assert.Contains("UserRole", result);
        Assert.Contains("UserId: 1", result);
        Assert.Contains("RoleId: 2", result);
    }
}