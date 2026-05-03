namespace StarterApp.Database.Models;

/// <summary>
/// Represents a rental request between an item owner and a borrower.
/// Tracks the item, borrower, dates, total price, and current rental workflow status.
/// </summary>
public class Rental
{
    /// <summary>
    /// Unique rental identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifier of the item being rented.
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// The item associated with this rental.
    /// </summary>
    public Item? Item { get; set; }
    
    /// <summary>
    /// Identifier of the user requesting the rental.
    /// </summary>
    public int BorrowerId { get; set; }

     /// <summary>
    /// The user requesting the rental.
    /// </summary>
    public User? Borrower { get; set; }
    
    /// <summary>
    /// Rental start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Rental end date.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Total rental cost calculated from daily rate and duration.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Current rental workflow status.
    /// </summary>
    public RentalStatus Status { get; set; } = RentalStatus.Requested;

    /// <summary>
    /// Returns true when the rental is awaiting owner approval.
    /// </summary>
    public bool IsRequested => Status == RentalStatus.Requested;

    /// <summary>
    /// Returns true when the rental has been approved by the owner.
    /// </summary>
    public bool IsApproved => Status == RentalStatus.Approved;

    /// <summary>
    /// Returns true when the item is currently out for rent.
    /// </summary>
    public bool IsOutForRent => Status == RentalStatus.OutForRent;

    /// <summary>
    /// Returns true when the borrower has marked the item as returned.
    /// </summary>
    public bool IsReturned => Status == RentalStatus.Returned;

    /// <summary>
    /// Returns true when the owner has completed the rental.
    /// </summary>
    public bool IsCompleted => Status == RentalStatus.Completed;

    /// <summary>
    /// Date and time when the rental was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    
}