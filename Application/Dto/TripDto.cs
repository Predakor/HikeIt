using Domain.Entiites.Regions;
using Domain.TripAnalytics;
using Domain.Trips.Entities.GpxFiles;

namespace Application.Dto;

/// <summary>
/// Core trip properties shared by all trip DTOs.
/// </summary>
public record TripBase(string Name, DateOnly TripDay);

public record TripBasePartial(float? Height, float? Distance, float? Duration, DateOnly? TripDay);

public abstract record TripDto(TripBase Base) {
    public record Partial(
        Guid Id,
        TripAnalyticsDto.Full? TrackAnalytic,
        GpxFile? GpxFile,
        Region? Region,
        TripBase Base
    ) : TripDto(Base);

    public record Partial2(
        Guid Id,
        TripAnalytic? TrackAnalytic,
        GpxFile? GpxFile,
        Region? Region,
        TripBase Base
    ) : TripDto(Base);


    public record PartialLinked(
        TripAnalytic? TrackAnalytic,
        Guid? GpxFileId,
        int? RegionId,
        TripBasePartial? Base
    );

    public record Request(Guid Id, TripBase Base) : TripDto(Base) {
        public record ResponseBasic(Guid Id, int RegionId, TripBase Base) : Request(Id, Base);

        public record Response(
            Guid Id,
            RegionDto.Complete? Region,
            GpxFile? GpxFile,
            TripAnalytic? TrackAnalytic,
            TripBase Base
        ) : Request(Id, Base);

        /// <summary>
        /// Full response with required region, file ID, and analytics.
        /// </summary>
        public record ResponseFull(
            Guid Id,
            RegionDto.Complete Region,
            GpxFile GpxFile,
            TripAnalytic TrackAnalytic,
            TripBase Base
        ) : Request(Id, Base);

        /// <summary>
        /// DTO for updating trip information.
        /// </summary>
        public record Update(Guid Id, int? RegionId, Guid? GpxFileId, TripBasePartial? Base);

        public record Create(int RegionId, TripBase Base) : TripDto(Base);

        public record Delete(Guid Id);
    }
}
