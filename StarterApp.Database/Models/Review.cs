namespace StarterApp.Database.Models;

/// <summary>
/// Represents feedback left by a user after completing a rental.
/// Reviews are linked to an item and reviewer and include a rating and written comment.
/// </summary>
public class Review
{
    /// <summary>
    /// Unique review identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifier of the item being reviewed.
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// The item associated with this review.
    /// </summary>
    public Item? Item { get; set; }

    /// <summary>
    /// Identifier of the user who submitted the review.
    /// </summary>
    public int ReviewerId { get; set; }

    /// <summary>
    /// The user who submitted the review.
    /// </summary>
    public User? Reviewer { get; set; }

    /// <summary>
    /// Numeric rating given by the reviewer, typically from 1 to 5.
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Written feedback provided by the reviewer.
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the review was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}