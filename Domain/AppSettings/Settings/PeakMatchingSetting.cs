using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;

namespace Domain.AppSettings.Settings;

public sealed record PeakMatchingSetting(IReadOnlyList<IPeakFilterConfig> Filters)
    : IAppSetting<PeakMatchingSetting>,
        IFilterSetting<IPeakFilterConfig>
{
    public static AppSettingType SettingFor => AppSettingType.PeakMatching;

    public static PeakMatchingSetting GetDefault() => new(PipelineDefaults.PeakFilters);

    public IEnumerable<string> Validate()
    {
        var filters = Filters ?? [];

        int localMaximaIndex = filters
            .ToList()
            .FindIndex(filter => filter is LocalMaximaFilter.Config);
        bool localMaximaNotFirst = localMaximaIndex > 0;
        if (localMaximaNotFirst)
        {
            yield return "LocalMaximaFilter must be the first filter: it needs the full track.";
        }

        foreach (var filter in filters)
        {
            if (filter is null)
            {
                yield return "Filters must not contain empty entries.";
            }

            if (filter is LocalMaximaFilter.Config { MinClimbM: < 0 })
            {
                yield return "LocalMaximaFilter MinClimbM must not be negative.";
            }

            if (filter is PeakLookupFilter.Config { OffsetM: < 0 })
            {
                yield return "PeakLookupFilter OffsetM must not be negative.";
            }

            if (filter is MatchWithinDistanceFilter.Config { MaxDistanceM: <= 0 })
            {
                yield return "MatchWithinDistanceFilter MaxDistanceM must be greater than 0.";
            }
        }

        var lookupOffset = filters
            .OfType<PeakLookupFilter.Config>()
            .Select(lookup => lookup.OffsetM)
            .DefaultIfEmpty()
            .Max();
        var largestMatchDistance = filters
            .OfType<MatchWithinDistanceFilter.Config>()
            .Select(match => match.MaxDistanceM)
            .DefaultIfEmpty()
            .Max();
        bool lookupBoxTooSmall =
            filters.Any(filter => filter is PeakLookupFilter.Config) && lookupOffset < largestMatchDistance;
        if (lookupBoxTooSmall)
        {
            yield return "PeakLookupFilter OffsetM must be at least MatchWithinDistanceFilter MaxDistanceM, or peaks inside the match distance can be missed.";
        }
    }
}
