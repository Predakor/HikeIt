using Application.AppSettings;
using Application.Commons.Abstractions;
using Application.Locations.Peaks;
using Application.ReachedPeaks.ValueObjects;
using Application.Trips.Analytics.Pipeline;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;
using Domain.ReachedPeaks;
using Domain.ReachedPeaks.Builders;
using Domain.ReachedPeaks.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles.Commands;
using Domain.Trips.Root;
using Domain.Users.Root;

namespace Application.ReachedPeaks;

public class ReachedPeakService : IReachedPeakService {
    readonly IPeaksQueryService _peaksQueries;
    readonly IReachedPeaksQureryService _reachedPeaksQueries;
    readonly IAppSettingsService _appSettings;

    public ReachedPeakService(
        IPeaksQueryService peaksQueryService,
        IReachedPeaksQureryService reachedPeaksQueries,
        IAppSettingsService appSettings
    ) {
        _peaksQueries = peaksQueryService;
        _reachedPeaksQueries = reachedPeaksQueries;
        _appSettings = appSettings;
    }

    public Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user) {
        return ReachedPeak.Create(peak, trip, user);
    }

    public async Task<Result<List<ReachedPeak>>> CreateReachedPeaks(AnalyticData data, Trip trip) {
        // Read once per trip; a missing or unreadable row falls back to the code constants.
        var setting = await PipelineSettings.PeakMatchingAsync(_appSettings, CancellationToken.None);
        var filters = setting is null ? PipelineDefaults.PeakFilters : PipelineDefaults.PeakFiltersFrom(setting);
        var radius = setting?.ProximityRadiusM ?? PipelineDefaults.PeakProximityRadiusM;

        return await ExtractPotentialPeaks(data, filters)
            .BindAsync(candidates => FindMatchingPeaks(candidates, radius))
            .BindAsync(peaks => MarkNewPeaks(trip.UserId, peaks))
            .BindAsync(foundPeaks => ToReachedPeaks(foundPeaks, trip));
    }

    static Result<List<ReachedPeakDataBuilder>> ExtractPotentialPeaks(
        AnalyticData data,
        IReadOnlyList<IPeakFilterConfig> filters
    ) {
        return PeakFilterFactory
            .Apply(data.ToLocalMaximaWithDistance(), filters)
            .Select(ReachedPeakDataFactory.FromPointWithDistance)
            .ToList();
    }

    Task<Result<List<ReachedPeakDataBuilder>>> FindMatchingPeaks(
        List<ReachedPeakDataBuilder> peaksCandidates,
        float radius
    ) {
        return _peaksQueries.GetPeaksWithinRadius(peaksCandidates, radius);
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
