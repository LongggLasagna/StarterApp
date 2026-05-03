namespace StarterApp.Database.Models;

/// <summary>
/// Represents an item that can be listed, viewed, searched, and rented by users.
/// Includes pricing, category, owner, and location information.
/// </summary>
public class Item
{
    /// <summary>
    /// Unique item identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Item title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Item description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    public decimal DailyRate { get; set; }

    public string Category { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Latitude coordinate used by nearby item search.
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude coordinate used by nearby item search.
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Indicates whether the item is currently available for rent.
    /// </summary>
    public bool IsAvailable { get; set; }

    public int OwnerId { get; set; }

    public User? Owner { get; set; }
}