using Domain.Common.Abstractions;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

public sealed class RoundingPrecisionFilter : IFilter<MutableGpxPoint>
{
    // Targets omitted from JSON means Elevation, which is what production rounds.
    public sealed record Config(int Decimals, PointFields Targets = PointFields.Elevation) : IPointFilterConfig
    {
        public IFilter<MutableGpxPoint> CreateFilter() => new RoundingPrecisionFilter(this);
    }

    private readonly int _decimals;
    private readonly PointFields _targets;

    public RoundingPrecisionFilter(Config config)
    {
        config.Targets.EnsureAny(nameof(config));

        _decimals = config.Decimals;
        _targets = config.Targets;
    }

    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        foreach (var point in values)
        {
            if (_targets.HasFlag(PointFields.Elevation))
            {
                point.Ele = Math.Round(point.Ele, _decimals);
            }

            if (_targets.HasFlag(PointFields.Latitude))
            {
                point.Lat = Math.Round(point.Lat, _decimals);
            }

            if (_targets.HasFlag(PointFields.Longitude))
            {
                point.Lon = Math.Round(point.Lon, _decimals);
            }
        }

        return values;
    }
}