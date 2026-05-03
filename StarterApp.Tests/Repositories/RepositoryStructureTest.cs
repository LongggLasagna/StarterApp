using StarterApp.Database.Data.Repositories;
using Xunit;

namespace StarterApp.Tests.Repositories;

public class RepositoryStructureTest
{
    [Fact]
    public void ItemRepository_InterfaceExists()
    {
        var type = typeof(IItemRepository);

        Assert.NotNull(type);
        Assert.True(type.IsInterface);
    }

    [Fact]
    public void RentalRepository_InterfaceExists()
    {
        var type = typeof(IRentalRepository);

        Assert.NotNull(type);
        Assert.True(type.IsInterface);
    }

    [Fact]
    public void ReviewRepository_InterfaceExists()
    {
        var type = typeof(IReviewRepository);

        Assert.NotNull(type);
        Assert.True(type.IsInterface);
    }
}

