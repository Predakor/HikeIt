using Application.Locations.Peaks;
using Application.ReachedPeaks.ValueObjects;
using Domain.Common.Geography.ValueObjects;
using Domain.Locations.Peaks;
using Domain.ReachedPeaks;
using Domain.ReachedPeaks.Builders;
using Domain.ReachedPeaks.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles.Commands;
using Domain.Trips.Root;
using Domain.Users.Root;

namespace Application.ReachedPeaks;

public class ReachedPeakService : IReachedPeakService {
    const float PeakProximityTreshold = 100f;
    readonly IPeaksQueryService _peaksQueries;
    readonly IReachedPeaksQureryService _reachedPeaksQueries;

    public ReachedPeakService(
        IPeaksQueryService peaksQueryService,
        IReachedPeaksQureryService reachedPeaksQueries
    ) {
        _peaksQueries = peaksQueryService;
        _reachedPeaksQueries = reachedPeaksQueries;
    }

    public Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user) {
        return ReachedPeak.Create(peak, trip, user);
    }

    public async Task<Result<List<ReachedPeak>>> CreateReachedPeaks(AnalyticData data, Trip trip) {
        return await ExtractPotentialPeaks(data)
            .BindAsync(FindMatchingPeaks)
            .BindAsync(peaks => MarkNewPeaks(trip.UserId, peaks))
            .BindAsync(foundPeaks => ToReachedPeaks(foundPeaks, trip));
    }

    static Result<List<ReachedPeakDataBuilder>> ExtractPotentialPeaks(AnalyticData data) {
        return data.ToLocalMaximaWithDistance()
            .WithProximityMerge()
            .Select(ReachedPeakDataFactory.FromPointWithDistance)
            .ToList();
    }

    Task<Result<List<ReachedPeakDataBuilder>>> FindMatchingPeaks(
        List<ReachedPeakDataBuilder> peaksCandidates
    ) {
        return _peaksQueries.GetPeaksWithinRadius(peaksCandidates, PeakProximityTreshold);
    }

    async Task<Result<List<ReachedPeakDataBuilder>>> MarkNewPeaks(
        Guid userId,
        List<ReachedPeakDataBuilder> peaks
    ) {
        var newPeaks = await _reachedPeaksQueries.ReachedByUserBefore(
            userId,
            peaks.Select(p => p.PeakId)
        );

        if (newPeaks.NullOrEmpty()) {
            return peaks;
        }

        foreach (var newPeak in newPeaks) {
            var firstMathchingPeak = peaks.FirstOrDefault(p => p.PeakId == newPeak.Id);
            if (firstMathchingPeak is not null) {
                firstMathchingPeak.SetFirstTimeReached(true);
            }
        }

        return peaks;
    }

    static Result<List<ReachedPeak>> ToReachedPeaks(
        IEnumerable<ReachedPeakDataBuilder> peaks,
        Trip trip
    ) {
        if (peaks.NullOrEmpty()) {
            return Errors.EmptyCollection("Peaks");
        }

        Console.WriteLine($"Matched {peaks.Count()} Peaks");

        CreateReachedPeak[] newPeaksEventData = [.. peaks.Select(p => p.Build())];
        trip.AddReachedPeaks(newPeaksEventData);

        return peaks
            .Select(p =>
                ReachedPeak.Create(p.PeakId, trip.Id, trip.UserId, p.TimeReached, p.FirstTime)
            )
            .ToList();
    }
}
