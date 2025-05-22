namespace Application.Dto;

public abstract record TripDto {
    public record Basic(float Height, float Length, float Duration) : TripDto;

    public record CompleteLinked(
        float Height,
        float Length,
        float Duration,
        int RegionId,
        DateOnly TripDay
    ) : TripDto;

    public record Complete(
        float Height,
        float Length,
        float Duration,
        RegionDto.Complete Region,
        DateOnly TripDay
    ) : TripDto;

    public record UpdateDto(
        int Id,
        float Height,
        float Length,
        float Duration,
        int RegionId,
        DateOnly TripDay
    ) : TripDto;
}
