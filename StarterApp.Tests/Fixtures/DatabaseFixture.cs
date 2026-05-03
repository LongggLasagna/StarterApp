using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;

namespace StarterApp.Tests.Fixtures;

/// <summary>
/// Provides a seeded in-memory database for repository integration-style tests.
/// </summary>
public class DatabaseFixture : IDisposable
{
    private readonly DbContextOptions<AppDbContext> _options;

    public DatabaseFixture()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"StarterAppTests_{Guid.NewGuid()}")
            .Options;

        ResetDatabase();
    }

    public AppDbContext CreateContext()
    {
        return new AppDbContext(_options);
    }

    public void ResetDatabase()
    {
        using var context = CreateContext();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        SeedTestData(context);
    }

    private static void SeedTestData(AppDbContext context)
    {
        var owner = new User
        {
            Id = 1,
            FirstName = "Owner",
            LastName = "User",
            Email = "owner@test.com",
            PasswordHash = "hash",
            PasswordSalt = "salt",
            IsActive = true
        };

        var borrower = new User
        {
            Id = 2,
            FirstName = "Borrower",
            LastName = "User",
            Email = "borrower@test.com",
            PasswordHash = "hash",
            PasswordSalt = "salt",
            IsActive = true
        };

        var itemOne = new Item
        {
            Id = 1,
            Title = "Camera",
            Description = "DSLR camera",
            DailyRate = 15,
            CategoryId = 1,
            Category = "Electronics",
            Location = "Edinburgh",
            Latitude = 55.9533,
            Longitude = -3.1883,
            OwnerId = owner.Id,
            Owner = owner,
            IsAvailable = true
        };

        var itemTwo = new Item
        {
            Id = 2,
            Title = "Drill",
            Description = "Cordless drill",
            DailyRate = 10,
            CategoryId = 2,
            Category = "Tools",
            Location = "Edinburgh",
            Latitude = 55.9500,
            Longitude = -3.1800,
            OwnerId = owner.Id,
            Owner = owner,
            IsAvailable = true
        };

        var approvedRental = new Rental
        {
            Id = 1,
            ItemId = itemOne.Id,
            Item = itemOne,
            BorrowerId = borrower.Id,
            Borrower = borrower,
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2026, 1, 5),
            TotalPrice = 60,
            Status = RentalStatus.Approved,
            CreatedAt = new DateTime(2026, 1, 1)
        };

        var completedRental = new Rental
        {
            Id = 2,
            ItemId = itemTwo.Id,
            Item = itemTwo,
            BorrowerId = borrower.Id,
            Borrower = borrower,
            StartDate = new DateTime(2026, 2, 1),
            EndDate = new DateTime(2026, 2, 3),
            TotalPrice = 20,
            Status = RentalStatus.Completed,
            CreatedAt = new DateTime(2026, 2, 1)
        };

        var review = new Review
        {
            Id = 1,
            ItemId = itemTwo.Id,
            Item = itemTwo,
            ReviewerId = borrower.Id,
            Reviewer = borrower,
            Rating = 5,
            Comment = "Great item",
            CreatedAt = new DateTime(2026, 2, 4)
        };

        context.Users.AddRange(owner, borrower);
        context.Items.AddRange(itemOne, itemTwo);
        context.Rentals.AddRange(approvedRental, completedRental);
        context.Reviews.Add(review);

        context.SaveChanges();
    }

    public void Dispose()
    {
        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}