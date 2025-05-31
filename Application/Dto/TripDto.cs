using Domain.GpxFiles;
using Domain.Regions;
using Domain.TrackAnalytics;

namespace Application.Dto;

/// <summary>
/// Core trip properties shared by all trip DTOs.
/// </summary>
public record TripBase(float Height, float Distance, float Duration, DateOnly TripDay);

public record TripBasePartial(float? Height, float? Distance, float? Duration, DateOnly? TripDay);

public abstract record TripDto(TripBase Base) {
    public record Partial(
        int Id,
        TrackAnalytic? TrackAnalytic,
        GpxFile? GpxFile,
        Region? Region,
        TripBase Base
    ) : TripDto(Base);

    public record PartialLinked(
        int Id,
        TrackAnalytic? TrackAnalytic,
        Guid? GpxFileId,
        int? RegionId,
        TripBase Base
    ) : TripDto(Base);

    public record Request(int Id, TripBase Base) : TripDto(Base) {
        public record ResponseBasic(int Id, int RegionId, TripBase Base) : Request(Id, Base);

        public record Response(
            int Id,
            RegionDto.Complete? Region,
            GpxFile? GpxFile,
            TrackAnalytic? TrackAnalytic,
            TripBase Base
        ) : Request(Id, Base);

        /// <summary>
        /// Full response with required region, file ID, and analytics.
        /// </summary>
        public record ResponseFull(
            int Id,
            RegionDto.Complete Region,
            GpxFile GpxFile,
            TrackAnalytic TrackAnalytic,
            TripBase Base
        ) : Request(Id, Base);

        /// <summary>
        /// DTO for updating trip information.
        /// </summary>
        public record Update(int Id, int? RegionId, TripBasePartial? Base);

        public record Create(int RegionId, TripBase Base) : TripDto(Base);

        public record Delete(int Id);
    }
}
