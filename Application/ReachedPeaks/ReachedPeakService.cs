using Application.Mountains;
using Domain.Common;
using Domain.Common.Result;
using Domain.Mountains.Peaks;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Commands;
using Domain.Trips;
using Domain.Trips.ValueObjects;
using Domain.Users;

namespace Application.ReachedPeaks;

public class ReachedPeakService : IReachedPeakService {
    static readonly float PeakProximityTreshold = 600f;
    readonly IPeaksQueryService _peaksQueries;

    public ReachedPeakService(IPeaksQueryService peaksQueryService) {
        _peaksQueries = peaksQueryService;
    }

    public async Task<Result<List<ReachedPeak>>> GetPeaks(
        AnalyticData data,
        Guid tripId,
        Guid userId
    ) {
        var peaksCandidates = data.ToLocalMaximaWithMerges();

        return await _peaksQueries
            .GetPeaksWithinRadius(peaksCandidates, PeakProximityTreshold)
            .BindAsync(foundPeaks => ToReachedPeaks(foundPeaks, tripId, userId));
    }

    public Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user) {
        return ReachedPeak.Create(peak, trip, user);
    }

    public Result<List<ReachedPeak>> ToReachedPeaks(
        IEnumerable<Peak> peaks,
        Guid tripId,
        Guid userId
    ) {
        if (!peaks.Any()) {
            return Errors.EmptyCollection("Peaks");
        }

        Console.WriteLine($"Matched {peaks.Count()} Peaks");

        return peaks.Select(p => ReachedPeak.Create(p.Id, tripId, userId)).ToList();
    }
}
