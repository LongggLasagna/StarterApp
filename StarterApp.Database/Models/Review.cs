namespace StarterApp.Database.Models;

public class Review
{
    public int Id { get; set; }

    public int ItemId { get; set; }
    public Item? Item { get; set; }

    public int ReviewerId { get; set; }
    public User? Reviewer { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}