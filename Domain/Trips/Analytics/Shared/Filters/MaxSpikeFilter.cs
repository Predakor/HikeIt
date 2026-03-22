using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

public sealed class MaxSpikeFilter(MaxSpikeFilter.Config config) : IFilter<MutableGpxPoint>
{
    public sealed record Config(int MaxSpike) : FilterConfigBase<Config>(default!);

    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        if (values.Count < 2)
        {
            return values;
        }

        double lastValidEle = values[0].Ele;

        for (int i = 1; i < values.Count; i++)
        {
            var current = values[i];
            double diff = current.Ele - lastValidEle;
            double clampedDiff = Math.Clamp(diff, -config.MaxSpike, config.MaxSpike);

            lastValidEle = lastValidEle + clampedDiff;
            current.Ele = lastValidEle;
        }

        return values;
    }

}