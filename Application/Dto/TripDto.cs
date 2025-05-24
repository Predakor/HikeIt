namespace Application.Dto;

public abstract record TripDto {
    public record Basic(float Height, float Length, float Distance) : TripDto;

    public record CompleteLinked(
        float Height,
        float Length,
        float Distance,
        int RegionId,
        DateOnly TripDay
    ) : TripDto;

    public record Complete(
        float Height,
        float Length,
        float Distance,
        RegionDto.Complete Region,
        DateOnly TripDay
    ) : TripDto;

    public record UpdateDto(
        int Id,
        float Height,
        float Length,
        float Distance,
        int RegionId,
        DateOnly TripDay
    ) : TripDto;
}
