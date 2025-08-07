namespace Application.Trips;

public sealed record UpdateTripDto {
    public string? TripName { get; init; }
    public DateOnly? TripDay { get; init; }
    public int? RegionId { get; init; }
}
