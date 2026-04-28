using System.Net.Http.Json;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.Services;

public class ApiRentalRepository : IRentalRepository
{
    private readonly HttpClient _httpClient;

    public ApiRentalRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

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

    public async Task<Rental?> GetByIdAsync(int id)
    {
        var dto = await _httpClient.GetFromJsonAsync<ApiRentalDto>($"rentals/{id}");
        return dto == null ? null : ToRental(dto);
    }

    public async Task UpdateAsync(Rental rental)
    {
        var request = new
        {
            status = rental.Status.ToString()
        };

        var response = await _httpClient.PatchAsJsonAsync($"rentals/{rental.Id}/status", request);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Failed to update rental status: {body}");
        }
    }

    public async Task<List<Rental>> GetIncomingAsync(int ownerId)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiRentalsResponse>("rentals/incoming");
        return response?.Rentals.Select(ToRental).ToList() ?? new List<Rental>();
    }

    public async Task<List<Rental>> GetOutgoingAsync(int borrowerId)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiRentalsResponse>("rentals/outgoing");
        return response?.Rentals.Select(ToRental).ToList() ?? new List<Rental>();
    }

    public Task<bool> HasOverLappingRentalAsync(int itemId, DateTime startDate, DateTime endDate)
    {
        // The shared API validates overlapping rentals and returns 409 Conflict.
        return Task.FromResult(false);
    }

    public Task<bool> HasCompletedRentalAsync(int itemId, int borrowerId)
    {
        // Review locking is enforced by the shared API when creating reviews.
        return Task.FromResult(true);
    }

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

    private static RentalStatus ParseStatus(string status)
    {
        return status.Replace(" ", "") switch
        {
            "Requested" => RentalStatus.Requested,
            "Approved" => RentalStatus.Approved,
            "Rejected" => RentalStatus.Rejected,
            "OutForRent" => RentalStatus.OutForRent,
            "Returned" => RentalStatus.Returned,
            "Completed" => RentalStatus.Completed,
            _ => RentalStatus.Requested
        };
    }
}

public record ApiRentalsResponse(
    List<ApiRentalDto> Rentals,
    int TotalRentals);

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