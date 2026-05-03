using System.Net.Http.Json;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// API-backed implementation of the rental repository.
/// Handles rental creation, status updates, incoming requests, and outgoing requests through the shared backend API.
/// </summary>
public class ApiRentalRepository : IRentalRepository
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Creates a new API rental repository using the configured HTTP client.
    /// </summary>
    /// <param name="httpClient">The HTTP client configured with the backend base URL and authorization token.</param>
    public ApiRentalRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Creates a new rental request through the backend API.
    /// </summary>
    /// <param name="rental">The rental request containing item and date information.</param>
    /// <exception cref="InvalidOperationException">Thrown when the API rejects the rental request.</exception>
    public async Task AddAsync(Rental rental)
    {
        var request = new
        {
            itemId = rental.ItemId,
            startDate = rental.StartDate.ToString("yyyy-MM-dd"),
            endDate = rental.EndDate.ToString("yyyy-MM-dd")
        };

        var response = await _httpClient.PostAsJsonAsync("rentals", request);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to request rental: {body}");
        }
    }

    /// <summary>
    /// Retrieves a rental by its identifier from the backend API.
    /// </summary>
    /// <param name="id">The rental identifier.</param>
    /// <returns>The matching rental if found; otherwise null.</returns>
    public async Task<Rental?> GetByIdAsync(int id)
    {
        var dto = await _httpClient.GetFromJsonAsync<ApiRentalDto>($"rentals/{id}");
        return dto == null ? null : ToRental(dto);
    }

    /// <summary>
    /// Updates the status of a rental through the backend API.
    /// </summary>
    /// <param name="rental">The rental containing the updated status.</param>
    /// <exception cref="InvalidOperationException">Thrown when the API rejects the status transition.</exception>
    public async Task UpdateAsync(Rental rental)
    {
        var request = new
        {
            status = ToApiStatus(rental.Status)
        };

        var response = await _httpClient.PatchAsJsonAsync($"rentals/{rental.Id}/status", request);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to update rental status: {body}");
        }
    }

    /// <summary>
    /// Retrieves rental requests made by other users for items owned by the current authenticated user.
    /// </summary>
    /// <param name="ownerId">The owner identifier. The API uses the authenticated token to identify the user.</param>
    /// <returns>A list of incoming rental requests.</returns>
    public async Task<List<Rental>> GetIncomingAsync(int ownerId)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiRentalsResponse>("rentals/incoming");
        return response?.rentals.Select(ToRental).ToList() ?? new List<Rental>();
    }

    /// <summary>
    /// Retrieves rental requests created by the current authenticated user.
    /// </summary>
    /// <param name="borrowerId">The borrower identifier. The API uses the authenticated token to identify the user.</param>
    /// <returns>A list of outgoing rental requests.</returns>
    public async Task<List<Rental>> GetOutgoingAsync(int borrowerId)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiRentalsResponse>("rentals/outgoing");
        return response?.rentals.Select(ToRental).ToList() ?? new List<Rental>();
    }

    /// <summary>
    /// Checks whether an item has an overlapping rental for the requested dates.
    /// The shared API performs this validation when rental requests are created.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="startDate">The proposed rental start date.</param>
    /// <param name="endDate">The proposed rental end date.</param>
    /// <returns>False because overlap validation is delegated to the backend API.</returns>
    public Task<bool> HasOverLappingRentalAsync(int itemId, DateTime startDate, DateTime endDate)
    {
        return Task.FromResult(false);
    }

    /// <summary>
    /// Checks whether a borrower has completed a rental for an item.
    /// Review eligibility is enforced by the shared API when reviews are submitted.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="borrowerId">The borrower identifier.</param>
    /// <returns>True because review locking is delegated to the backend API.</returns>
    public Task<bool> HasCompletedRentalAsync(int itemId, int borrowerId)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Maps an API rental data transfer object into the local Rental model.
    /// </summary>
    /// <param name="dto">The rental data transfer object returned by the backend API.</param>
    /// <returns>The mapped rental model.</returns>
    private static Rental ToRental(ApiRentalDto dto)
    {
        return new Rental
        {
            Id = dto.Id,
            ItemId = dto.ItemId,
            Item = new Item
            {
                Id = dto.ItemId,
                Title = dto.ItemTitle
            },
            BorrowerId = dto.BorrowerId,
            Borrower = new User
            {
                Id = dto.BorrowerId,
                Email = dto.BorrowerName
            },
            StartDate = DateTime.Parse(dto.StartDate),
            EndDate = DateTime.Parse(dto.EndDate),
            Status = ParseStatus(dto.Status),
            TotalPrice = dto.TotalPrice,
            CreatedAt = dto.RequestedAt ?? dto.CreatedAt ?? DateTime.UtcNow
        };
    }

    /// <summary>
    /// Converts the API rental status text into the local RentalStatus enum.
    /// Handles spacing, hyphens, underscores, and casing differences from the API.
    /// </summary>
    /// <param name="status">The status string returned by the backend API.</param>
    /// <returns>The matching RentalStatus enum value.</returns>
    private static RentalStatus ParseStatus(string status)
    {
        var normalised = status
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("_", "")
            .ToLowerInvariant();

        return normalised switch
        {
            "requested" => RentalStatus.Requested,
            "approved" => RentalStatus.Approved,
            "rejected" => RentalStatus.Rejected,
            "outforrent" => RentalStatus.OutForRent,
            "returned" => RentalStatus.Returned,
            "completed" => RentalStatus.Completed,
            _ => RentalStatus.Requested
        };
    }

    /// <summary>
    /// Converts the local RentalStatus enum into the exact status string expected by the backend API.
    /// </summary>
    /// <param name="status">The local rental status.</param>
    /// <returns>The API-compatible status text.</returns>
    private static string ToApiStatus(RentalStatus status)
    {
        return status switch
        {
            RentalStatus.Requested => "Requested",
            RentalStatus.Approved => "Approved",
            RentalStatus.Rejected => "Rejected",
            RentalStatus.OutForRent => "Out for Rent",
            RentalStatus.Returned => "Returned",
            RentalStatus.Completed => "Completed",
            _ => "Requested"
        };
    }
}

/// <summary>
/// Represents a response containing rental requests returned by the backend API.
/// </summary>
/// <param name="rentals">The rental requests returned by the API.</param>
/// <param name="totalRentals">The total number of rentals returned.</param>
public record ApiRentalsResponse(
    List<ApiRentalDto> rentals,
    int totalRentals);

/// <summary>
/// Represents a rental returned by the backend API before mapping to the local Rental model.
/// </summary>
/// <param name="Id">The rental identifier.</param>
/// <param name="ItemId">The rented item identifier.</param>
/// <param name="ItemTitle">The rented item title.</param>
/// <param name="BorrowerId">The borrower identifier.</param>
/// <param name="BorrowerName">The borrower display name.</param>
/// <param name="OwnerId">The owner identifier, if supplied by the API.</param>
/// <param name="OwnerName">The owner display name, if supplied by the API.</param>
/// <param name="StartDate">The rental start date.</param>
/// <param name="EndDate">The rental end date.</param>
/// <param name="Status">The rental status text returned by the API.</param>
/// <param name="TotalPrice">The calculated rental total.</param>
/// <param name="CreatedAt">The date and time the rental was created.</param>
/// <param name="RequestedAt">The date and time the rental was requested.</param>
public record ApiRentalDto(
    int Id,
    int ItemId,
    string ItemTitle,
    int BorrowerId,
    string BorrowerName,
    int? OwnerId,
    string? OwnerName,
    string StartDate,
    string EndDate,
    string Status,
    decimal TotalPrice,
    DateTime? CreatedAt,
    DateTime? RequestedAt);