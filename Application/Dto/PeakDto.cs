namespace Application.Dto;

public abstract record PeakDto {
    public sealed record Base(int Height, string Name, int Id);

    public sealed record Simple(int Height, string Name, int RegionID) : PeakDto;

    public sealed record Complete(int Height, string Name, RegionDto.Complete Region) : PeakDto;

    public sealed record WithLocation(int Id, int Height, string Name, double Lat, double Lon) : PeakDto;

    public sealed record Reached(int Id, int Height, DateTime? Time = null);

    public sealed record WithReachStatus(int Id, string Name, int Height, bool Reached);

    public sealed record CreateNew(
        string Name,
        int Height,
        double Lat,
        double Lon,
        int RegionId
    );

    public sealed record Update(
        string? Name,
        int? Height,
        double? Lat,
        double? Lon,
        int? RegionId
    );
}
