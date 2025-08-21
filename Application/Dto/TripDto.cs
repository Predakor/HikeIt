using Application.Dto.Analytics;
using Domain.FileReferences;
using Domain.Mountains.Regions;
using Domain.TripAnalytics;

namespace Application.Dto;

public record TripBase(string Name, DateOnly TripDay);
public abstract record TripDto(TripBase Base) {

    public record Summary(Guid Id, string Name, DateOnly TripDay, Region Region);

    public record WithBasicAnalytics(Guid Id, string Name, DateOnly TripDay, TripAnalyticsDto.Basic Analytics);

    public record Partial(
        Guid Id,
        TripAnalytic? TrackAnalytic,
        FileReference? GpxFile,
        Region? Region,
        TripBase Base
    ) : TripDto(Base);


    public record Request(Guid Id, TripBase Base) : TripDto(Base) {
        public record ResponseBasic(Guid Id, int RegionId, TripBase Base) : Request(Id, Base);

        public record Create(int RegionId, TripBase Base) : TripDto(Base);

        public record Delete(Guid Id);
    }
}
