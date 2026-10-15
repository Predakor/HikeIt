using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

// Ramer-Douglas-Peucker in local-plane coordinates (elevation as third axis). Never removes Pinned points.
public sealed class SimplifyFilter(SimplifyFilter.Config config) : IFilter<MutableGpxPoint>
{
    public sealed record Config(double ToleranceM) : FilterConfigBase<Config>(default!), IPointFilterConfig;

    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        // STUB (Phase 0): implemented by BE-2.
        throw new NotImplementedException();
    }
}
