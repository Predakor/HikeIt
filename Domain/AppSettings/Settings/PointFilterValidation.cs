using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;

namespace Domain.AppSettings.Settings;

internal static class PointFilterValidation
{
    public static IEnumerable<string> Validate(IReadOnlyList<IPointFilterConfig>? filters)
    {
        foreach (var filter in filters ?? [])
        {
            if (filter is null)
            {
                yield return "Filters must not contain empty entries.";
            }

            if (filter is MedianSmoothingFilter.Config { WindowSize: < 1 })
            {
                yield return "MedianSmoothingFilter WindowSize must be at least 1.";
            }

            if (filter is EmaSmoothingFilter.Config { Alpha: <= 0 or > 1 })
            {
                yield return "EmaSmoothingFilter Alpha must be greater than 0 and at most 1.";
            }

            if (filter is RoundingPrecisionFilter.Config { Decimals: < 0 })
            {
                yield return "RoundingPrecisionFilter Decimals must not be negative.";
            }

            if (filter is SimplifyFilter.Config { ToleranceM: < 0 })
            {
                yield return "SimplifyFilter ToleranceM must not be negative.";
            }

            if (filter is MedianSmoothingFilter.Config { Targets: PointFields.None }
                or EmaSmoothingFilter.Config { Targets: PointFields.None }
                or RoundingPrecisionFilter.Config { Targets: PointFields.None })
            {
                yield return "Smoothing and rounding filters must target Latitude, Longitude or Elevation.";
            }
        }
    }
}
