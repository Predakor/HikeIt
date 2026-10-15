using Application.AppSettings;
using Application.Locations.Peaks;
using Application.ReachedPeaks.ValueObjects;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Geography;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;
using Domain.ReachedPeaks.Builders;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Application.Trips.Analytics.Pipeline.Stages;

// Candidate peaks (local maxima, filtered) matched against the peak database. Read-only: no reached-peak
// flags, no Trip or User, no writes. Mirrors ReachedPeakService.CreateReachedPeaks up to the DB match.
public sealed class PeakMatchingStage(IPeaksQueryService peaksQueries, IAppSettingsService? appSettings = null) : IPipelineStage
{
    public StageId Id => StageId.PeakMatching;

    public object? SelectConfig(PipelineConfig config) => config.PeakMatching;

    // Default filters and radius follow the peak matching setting in the database; the code constants are the fallback.
    public async Task<string> CacheDiscriminatorAsync(CancellationToken cancellationToken)
    {
        var setting = await PipelineSettings.PeakMatchingAsync(appSettings, cancellationToken);
        if (setting is null)
        {
            return "defaults";
        }

        return FormattableString.Invariant(
            $"{setting.ProximityRadiusM}|{setting.MergeDistanceM}|{setting.MergeElevationM}"
        );
    }

    public async Task<StageOutput> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var data = CleanupStage.CleanedDataFrom(context);
        var setting = await PipelineSettings.PeakMatchingAsync(appSettings, cancellationToken);
        var defaultFilters = setting is null ? PipelineDefaults.PeakFilters : PipelineDefaults.PeakFiltersFrom(setting);
        var defaultRadius = setting?.ProximityRadiusM ?? PipelineDefaults.PeakProximityRadiusM;

        var filters = config.PeakMatching?.Filters ?? defaultFilters;
        var radius = config.PeakMatching?.ProximityRadiusM ?? defaultRadius;

        var candidates = PeakFilterFactory.Apply(data.ToLocalMaximaWithDistance(), filters).ToList();
        if (candidates.Count == 0)
        {
            return new StageOutput(new StageData.PeakMatching(0, []), 0);
        }

        var builders = candidates.Select(ReachedPeakDataFactory.FromPointWithDistance).ToList();
        var queried = await peaksQueries.GetPeaksWithinRadius(builders, radius);

        if (!queried.IsSuccess && !IsNoMatch(queried.Error!))
        {
            throw new InvalidOperationException(queried.Error!.Message);
        }

        List<ReachedPeakDataBuilder> matchedBuilders = queried.IsSuccess ? queried.Value! : [];

        var matched = new List<StageData.MatchedPeak>();
        foreach (var builder in matchedBuilders)
        {
            var peak = await FindPeakAsync(builder.PeakId, builder.Location, radius);
            var distanceM = DistanceHelpers.Distance2D(builder.Location, new GpxPoint(peak.Location.Y, peak.Location.X, 0));

            matched.Add(new StageData.MatchedPeak(builder.PeakId, peak.Name, distanceM, peak.Height));
        }

        return new StageOutput(new StageData.PeakMatching(candidates.Count, matched), matched.Count);
    }

    // The builder overload does not return peak locations, so the matched peak is looked up again around its candidate.
    async Task<Peak> FindPeakAsync(int peakId, GpxPoint candidate, float radius)
    {
        var nearby = await peaksQueries.GetPeaksWithinRadius(new[] { candidate }, radius);
        if (!nearby.IsSuccess)
        {
            throw new InvalidOperationException(nearby.Error!.Message);
        }

        var peak = nearby.Value!.FirstOrDefault(p => p.Id == peakId);
        if (peak is null)
        {
            throw new InvalidOperationException($"Peak {peakId} was matched but is not near its candidate.");
        }

        return peak;
    }

    // The DB lookup reports "no peak in radius" as NotFound; that is an empty match, not a failure.
    static bool IsNoMatch(ResultError error)
    {
        return error.Code == ErrorCode.not_found;
    }
}
