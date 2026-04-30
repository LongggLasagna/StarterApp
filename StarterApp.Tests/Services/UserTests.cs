using StarterApp.Database.Models;
using Xunit;

namespace StarterApp.Tests.Services;

public class UserTests
{
    [Fact]
    public void User_StoresEmailCorrectly()
    {
        var user = new User
        {
            Email = "test@test.com"
        };

        Assert.Equal("test@test.com", user.Email);
    }
}
