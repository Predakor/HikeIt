using Domain.Common.Geography.ValueObjects;
using Domain.ReachedPeaks.Builders;

namespace Application.ReachedPeaks.ValueObjects;

public static class ReachedPeakDataFactory {
    public static ReachedPeakDataBuilder FromPointWithDistance(GpxPointWithDistance point) {
        return ReachedPeakDataBuilder
            .Create(point.BasePoint)
            .WithDistanceFromStart(point.DistanceFromStart);
    }
}
