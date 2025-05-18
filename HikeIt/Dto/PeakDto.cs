using HikeIt.Api.Entities;

namespace HikeIt.Api.Dto;

public abstract record PeakDto(int Height, string Name) {
    public record New(int Height, string Name) : PeakDto(Height, Name);
    public record NewWithRegion(int Height, string Name, int RegionId) : PeakDto(Height, Name);
    public record NewWithCompleteRegion(int Height, string Name, Region Region) : PeakDto(Height, Name);

    public record Updated(int Id, int Height, string Name, int RegionId) : PeakDto(Height, Name);
}

