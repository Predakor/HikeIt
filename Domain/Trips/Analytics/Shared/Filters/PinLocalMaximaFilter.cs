using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.ElevationProfiles.Commands;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

// Marks local maxima (plus first and last point) as Pinned so later filters keep them.
public sealed class PinLocalMaximaFilter(PinLocalMaximaFilter.Config config) : SyncFilter<IList<MutableGpxPoint>>
{
    public sealed record Config(bool Enabled) : IPointFilterConfig
    {
        public IFilter<IList<MutableGpxPoint>> CreateFilter() => new PinLocalMaximaFilter(this);
    }

    public override IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        if (!config.Enabled || values.Count == 0)
        {
            return values;
        }

        values[0].Pinned = true;
        values[^1].Pinned = true;

        // ToGains needs at least two points.
        if (values.Count < 2)
        {
            return values;
        }

        var gpxPoints = values
            .Select(p => p.ToGpxPoint())
            .ToList();

        foreach (var localMaximum in AnalyticDataExtentions.LocalMaximaIndicesWithDistance(gpxPoints, null))
        {
            // The yielded index is the summit itself (same as ToLocalMaxima), gains[i] is the step from point i to i+1.
            values[localMaximum.index].Pinned = true;
        }

        return values;
    }
}
