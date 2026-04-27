namespace StarterApp.Services;

public record ApiErrorResponse(string Error, string Message);

public record ApiItemsResponse(
    List<ApiItemDto> Items,
    int TotalItems,
    int Page,
    int PageSize,
    int TotalPages);

public record ApiItemDto(
    int Id,
    string Title,
    string Description,
    decimal DailyRate,
    int CategoryId,
    string Category,
    int OwnerId,
    string OwnerName,
    double OwnerRating,
    bool IsAvailable,
    double AverageRating,
    DateTime CreatedAt,
    double? Latitude = null,
    double? Longitude = null,
    int TotalReviews = 0,
    List<ApiReviewDto>? Reviews = null);

public record ApiCreateItemRequest(
    string Title,
    string Description,
    decimal DailyRate,
    int CategoryId,
    double Latitude,
    double Longitude);

public record ApiUpdateItemRequest(
    string Title,
    string Description,
    decimal DailyRate,
    bool IsAvailable);

public record ApiReviewDto(
    int Id,
    int ReviewerId,
    string ReviewerName,
    int Rating,
    string Comment,
    DateTime CreatedAt);