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

    public ReachedPeakService(IPeaksQueryService peaksQueryService) {
        _peaksQueries = peaksQueryService;
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
            .BindAsync(foundPeaks => ToReachedPeaks(foundPeaks, tripId, userId));
    }

    static Result<List<CreateReachedPeakData>> ExtractPotentialPeaks(AnalyticData data) {
        return data.ToLocalMaxima()
            .WithProximityMerge()
            .Select(CreateReachedPeakData.FromGpxPoint)
            .ToList();
    }

    Task<Result<List<CreateReachedPeakData>>> FindMatchingPeaks(
        List<CreateReachedPeakData> peaksCandidates
    ) {
        return _peaksQueries.GetPeaksWithinRadius(peaksCandidates, PeakProximityTreshold);
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
