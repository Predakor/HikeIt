using Application.Dto.Analytics;
using Domain.Entiites.Regions;
using Domain.TripAnalytics;
using Domain.Trips.Entities.GpxFiles;

namespace Application.Dto;

/// <summary>
/// Core trip properties shared by all trip DTOs.
/// </summary>
public record TripBase(string Name, DateOnly TripDay);
public abstract record TripDto(TripBase Base) {

    public record Summary(Guid Id, string Name, DateOnly TripDay, Region Region);

    public record WithBasicAnalytics(Summary Summary, TripAnalyticsDto.Basic Analytics);

    public record Partial(
        Guid Id,
        TripAnalytic? TrackAnalytic,
        GpxFile? GpxFile,
        Region? Region,
        TripBase Base
    ) : TripDto(Base);


    public record Request(Guid Id, TripBase Base) : TripDto(Base) {
        public record ResponseBasic(Guid Id, int RegionId, TripBase Base) : Request(Id, Base);

        public record Create(int RegionId, TripBase Base) : TripDto(Base);

        public record Delete(Guid Id);
    }
}
