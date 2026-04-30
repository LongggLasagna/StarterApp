namespace StarterApp.Database.Models;

public class Item
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal DailyRate { get; set; }

    public string Category { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string Location { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
    public bool IsAvailable { get; set; }

    public int OwnerId { get; set; }

    public User? Owner { get; set; }
}