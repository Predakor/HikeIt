namespace Application.Trips;

public sealed record UpdateTripDto {
    public string? TripName;
    public DateOnly? TripDay;
}
