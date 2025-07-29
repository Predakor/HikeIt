using Application.Mountains;
using Application.ReachedPeaks.ValueObjects;
using Domain.Common;
using Domain.Common.GeographyHelpers;
using Domain.Common.Result;
using Domain.Common.Utils;
using Domain.Mountains.Peaks;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Commands;
using Domain.Trips;
using Domain.Trips.ValueObjects;
using Domain.Users;

namespace Application.ReachedPeaks;

public class ReachedPeakService : IReachedPeakService {
    static readonly float PeakProximityTreshold = GeoConverter.MetersToDegreesLatitude(300f);
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

    public async Task<Result<List<ReachedPeak>>> GetPeaks(
        AnalyticData data,
        Guid tripId,
        Guid userId
    ) {
        return await ExtractPotentialPeaks(data)
            .BindAsync(FindMatchingPeaks)
            .BindAsync(peaks => MarkNewPeaks(userId, peaks))
            .BindAsync(foundPeaks => ToReachedPeaks(foundPeaks, tripId, userId));
    }

    static Result<List<CreateReachedPeakData>> ExtractPotentialPeaks(AnalyticData data) {
        return data.ToLocalMaximaWithDistance()
            .WithProximityMerge()
            .Select(CreateReachedPeakData.FromGpxPoint)
            .ToList();
    }

    Task<Result<List<CreateReachedPeakData>>> FindMatchingPeaks(
        List<CreateReachedPeakData> peaksCandidates
    ) {
        return _peaksQueries.GetPeaksWithinRadius(peaksCandidates, PeakProximityTreshold);
    }

    async Task<Result<List<CreateReachedPeakData>>> MarkNewPeaks(
        Guid userId,
        List<CreateReachedPeakData> peaks
    ) {
        var newPeaksIds = await _reachedPeaksQueries.ReachedByUserBefore(
            userId,
            peaks.Select(p => p.PeakId)
        );

        if (newPeaksIds.NullOrEmpty()) {
            return peaks;
        }

        foreach (var newPeakId in newPeaksIds) {
            var firstMathchingPeak = peaks.FirstOrDefault(p => p.PeakId == newPeakId);
            if (firstMathchingPeak is not null) {
                firstMathchingPeak.FirstTime = true;
            }
        }

        return peaks;
    }

    static Result<List<ReachedPeak>> ToReachedPeaks(
        IEnumerable<CreateReachedPeakData> peaks,
        Guid tripId,
        Guid userId
    ) {
        if (!peaks.NotNullOrEmpty()) {
            return Errors.EmptyCollection("Peaks");
        }

        Console.WriteLine($"Matched {peaks.Count()} Peaks");

        return peaks
            .Select(p => ReachedPeak.Create(p.PeakId, tripId, userId, p.TimeReached, p.FirstTime))
            .ToList();
    }
}
