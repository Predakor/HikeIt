namespace Application.Dto;

public abstract record TripDto {
    public record Basic(float Height, float Duration, float Distance) : TripDto;

    public record CompleteLinked(
        float Height,
        float Duration,
        float Distance,
        int RegionId,
        DateOnly TripDay
    ) : TripDto;

    public record Complete(
        float Height,
        float Duration,
        float Distance,
        RegionDto.Complete Region,
        DateOnly TripDay
    ) : TripDto;

    public record UpdateDto(
        int Id,
        float Height,
        float Duration,
        float Distance,
        int RegionId,
        DateOnly TripDay
    ) : TripDto;
}
