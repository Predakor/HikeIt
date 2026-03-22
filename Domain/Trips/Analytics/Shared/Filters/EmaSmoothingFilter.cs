using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

public sealed class EmaSmoothingFilter : IFilter<MutableGpxPoint>
{
    public sealed record Config(double Alpha) : FilterConfigBase<Config>(default!);

    private readonly double _alpha;
    public EmaSmoothingFilter(Config config)
    {
        _alpha = config.Alpha;
    }

    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        var prevEma = values[0].Ele;
        var prev = values[0];

        foreach (var point in values.Skip(1))
        {

            prev.Ele = (_alpha * point.Ele) + ((1 - _alpha) * prev.Ele);
            prev.Lon = (_alpha * point.Lon) + ((1 - _alpha) * prev.Lon);
            prev.Lat = (_alpha * point.Lat) + ((1 - _alpha) * prev.Lat);

            point.Ele = prev.Ele;
            point.Lon = prev.Lon;
            point.Lat = prev.Lat;
        }

        return values;
    }

}
