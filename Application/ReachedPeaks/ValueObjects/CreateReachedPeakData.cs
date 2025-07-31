using Domain.ReachedPeaks.Builders;
using Domain.Trips.ValueObjects;

namespace Application.ReachedPeaks.ValueObjects;

public static class ReachedPeakDataFactory {
    public static ReachedPeakDataBuilder CreateFromGpxPointWithDistance(GpxPointWithDistance point) {
        return ReachedPeakDataBuilder
            .Create(point.BasePoint)
            .WithDistanceFromStart(point.DistanceFromStart);
    }
}
