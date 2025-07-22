using Application.Dto.Analytics;
using Application.TripAnalytics.Quries;
using Domain.Common;
using Domain.Common.Result;
using Domain.Trips.Config;
using Domain.Trips.ValueObjects;
using Infrastructure.Data;

namespace Infrastructure.TripAnalytics.Queries;

public class ElevationProfileQueryService : IElevationProfileQueryService {
    readonly TripDbContext _tripDbContext;

    public ElevationProfileQueryService(TripDbContext tripDbContext) {
        _tripDbContext = tripDbContext;
    }

    public Task<Result<ElevationProfileDto?>> DevAnalyticPreview(
        Guid id,
        ConfigBase.Nullable config
    ) {
        throw new NotImplementedException();
    }

    public async Task<Result<ElevationProfileDto>> GetElevationProfile(Guid id) {
        var query = await _tripDbContext.ElevationProfiles.FindAsync(id);

        if (query == null) {
            return Errors.NotFound("Elevation profile with id: " + id);
        }

        var scaledGains = ScaledGainSerializer.Deserialize(query.GainsData);

        var gains = Helpers.ToUnscaledGains(scaledGains);

        return new ElevationProfileDto(query.Start, gains);
    }

    static class Helpers {
        public static GainDto FromScaledGain(ScaledGain scaledGain) {
            return new GainDto(
                scaledGain.DistanceDelta,
                scaledGain.ElevationDelta,
                scaledGain.TimeDelta
            );
        }

        public static GainDto[] ToUnscaledGains(IEnumerable<ScaledGain> scaledGains) {
            return [.. scaledGains.Select(FromScaledGain)];
        }
    }
}
