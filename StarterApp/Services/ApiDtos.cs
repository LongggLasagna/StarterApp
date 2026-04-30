namespace StarterApp.Services;

/// <summary>
/// Represents a standard error response returned by the backend API.
/// </summary>
/// <param name="Error">The error type.</param>
/// <param name="Message">A human-readable error message.</param>
public record ApiErrorResponse(string Error, string Message);

/// <summary>
/// Represents a paginated response containing item listings.
/// </summary>
/// <param name="Items">The items returned for the current page.</param>
/// <param name="TotalItems">The total number of matching items.</param>
/// <param name="Page">The current page number.</param>
/// <param name="PageSize">The number of items per page.</param>
/// <param name="TotalPages">The total number of pages.</param>
public record ApiItemsResponse(
    List<ApiItemDto> Items,
    int TotalItems,
    int Page,
    int PageSize,
    int TotalPages);

/// <summary>
/// Represents the response from the nearby item discovery endpoint.
/// </summary>
/// <param name="items">The nearby items returned by the API.</param>
/// <param name="searchLocation">The latitude and longitude used for the search.</param>
/// <param name="radius">The radius used for the search in kilometres.</param>
/// <param name="totalResults">The number of nearby results returned.</param>
public record ApiNearbyItemsResponse(
    List<ApiItemDto> items,
    ApiSearchLocation searchLocation,
    double radius,
    int totalResults);

/// <summary>
/// Represents the geographic point used by a nearby item search.
/// </summary>
/// <param name="latitude">The latitude coordinate.</param>
/// <param name="longitude">The longitude coordinate.</param>
public record ApiSearchLocation(
    double latitude,
    double longitude);

/// <summary>
/// Represents an item returned by the backend API.
/// This DTO is mapped into the local Item model before being used by the UI.
/// </summary>
/// <param name="Id">The item identifier.</param>
/// <param name="Title">The item title.</param>
/// <param name="Description">The item description.</param>
/// <param name="DailyRate">The daily rental price.</param>
/// <param name="CategoryId">The category identifier.</param>
/// <param name="Category">The category display name.</param>
/// <param name="OwnerId">The owning user's identifier.</param>
/// <param name="OwnerName">The owner's display name.</param>
/// <param name="OwnerRating">The owner's average rating, if available.</param>
/// <param name="IsAvailable">Whether the item is currently available.</param>
/// <param name="AverageRating">The item's average rating, if available.</param>
/// <param name="CreatedAt">The date and time the item was created.</param>
/// <param name="Latitude">The item latitude coordinate.</param>
/// <param name="Longitude">The item longitude coordinate.</param>
/// <param name="TotalReviews">The number of reviews for the item.</param>
/// <param name="Reviews">Optional review data included with item details.</param>
public record ApiItemDto(
    int Id,
    string Title,
    string Description,
    decimal DailyRate,
    int CategoryId,
    string Category,
    int OwnerId,
    string OwnerName,
    double? OwnerRating,
    bool IsAvailable,
    double? AverageRating,
    DateTime CreatedAt,
    double? Latitude = null,
    double? Longitude = null,
    int TotalReviews = 0,
    List<ApiReviewDto>? Reviews = null);

/// <summary>
/// Represents the request body sent when creating an item listing.
/// </summary>
/// <param name="Title">The item title.</param>
/// <param name="Description">The item description.</param>
/// <param name="DailyRate">The daily rental price.</param>
/// <param name="CategoryId">The selected category identifier.</param>
/// <param name="Latitude">The listing latitude coordinate.</param>
/// <param name="Longitude">The listing longitude coordinate.</param>
public record ApiCreateItemRequest(
    string Title,
    string Description,
    decimal DailyRate,
    int CategoryId,
    double Latitude,
    double Longitude);

/// <summary>
/// Represents the request body sent when updating an existing item listing.
/// </summary>
/// <param name="Title">The updated item title.</param>
/// <param name="Description">The updated item description.</param>
/// <param name="DailyRate">The updated daily rental price.</param>
/// <param name="IsAvailable">Whether the item should be available for rental.</param>
public record ApiUpdateItemRequest(
    string Title,
    string Description,
    decimal DailyRate,
    bool IsAvailable);

/// <summary>
/// Represents a review returned by the backend API.
/// </summary>
/// <param name="Id">The review identifier.</param>
/// <param name="ReviewerId">The identifier of the reviewing user.</param>
/// <param name="ReviewerName">The reviewer's display name.</param>
/// <param name="Rating">The star rating from 1 to 5.</param>
/// <param name="Comment">The review comment.</param>
/// <param name="CreatedAt">The date and time the review was created.</param>
public record ApiReviewDto(
    int Id,
    int ReviewerId,
    string ReviewerName,
    int Rating,
    string Comment,
    DateTime CreatedAt);

/// <summary>
/// Represents a paginated response containing reviews for an item.
/// </summary>
/// <param name="reviews">The reviews returned for the current page.</param>
/// <param name="averageRating">The average rating for the item.</param>
/// <param name="totalReviews">The total number of reviews for the item.</param>
/// <param name="page">The current page number.</param>
/// <param name="pageSize">The number of reviews per page.</param>
/// <param name="totalPages">The total number of review pages.</param>
public record ApiItemReviewsResponse(
    List<ApiReviewDto> reviews,
    double? averageRating,
    int totalReviews,
    int page,
    int pageSize,
    int totalPages);