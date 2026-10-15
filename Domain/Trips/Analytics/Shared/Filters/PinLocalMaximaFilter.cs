using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

// Marks local maxima (plus first and last point) as Pinned so later filters keep them.
public sealed class PinLocalMaximaFilter(PinLocalMaximaFilter.Config config) : IFilter<MutableGpxPoint>
{
    public sealed record Config(bool Enabled) : FilterConfigBase<Config>(default!), IPointFilterConfig;

    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        // STUB (Phase 0): implemented by BE-2.
        throw new NotImplementedException();
    }
}
