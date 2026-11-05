using Domain.Peaks;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Root.Builders.GpxDataBuilder;
using System.Text.Json.Serialization;

namespace Domain.Common.Abstractions;

public interface IFilter<TValue>
{
    IList<TValue> Apply(IList<TValue> values);
}

public interface IPeakFilter
{
    ValueTask<IList<PeakCandidate>> ApplyAsync(IList<PeakCandidate> candidates);
}

public sealed record PeakFilterContext(IPeakSource Peaks);

[JsonPolymorphic(TypeDiscriminatorPropertyName = "FilterType")]
[JsonDerivedType(typeof(EmaSmoothingFilter.Config), typeDiscriminator: nameof(EmaSmoothingFilter))]
[JsonDerivedType(typeof(MaxSpikeFilter.Config), typeDiscriminator: nameof(MaxSpikeFilter))]
[JsonDerivedType(typeof(MedianSmoothingFilter.Config), typeDiscriminator: nameof(MedianSmoothingFilter))]
[JsonDerivedType(typeof(RoundingPrecisionFilter.Config), typeDiscriminator: nameof(RoundingPrecisionFilter))]
[JsonDerivedType(typeof(PinLocalMaximaFilter.Config), typeDiscriminator: nameof(PinLocalMaximaFilter))]
[JsonDerivedType(typeof(SimplifyFilter.Config), typeDiscriminator: nameof(SimplifyFilter))]
[JsonDerivedType(typeof(MergeNearbyByDistanceFilter.Config), typeDiscriminator: nameof(MergeNearbyByDistanceFilter))]
[JsonDerivedType(typeof(MergeNearbyByElevationFilter.Config), typeDiscriminator: nameof(MergeNearbyByElevationFilter))]
[JsonDerivedType(typeof(PeakLookupFilter.Config), typeDiscriminator: nameof(PeakLookupFilter))]
[JsonDerivedType(typeof(MatchWithinDistanceFilter.Config), typeDiscriminator: nameof(MatchWithinDistanceFilter))]
[JsonDerivedType(typeof(LocalMaximaFilter.Config), typeDiscriminator: nameof(LocalMaximaFilter))]
public interface IFilterConfig;

// Configs of filters that work on track points (cleanup, elevation profile, route).
[JsonPolymorphic(TypeDiscriminatorPropertyName = "FilterType")]
[JsonDerivedType(typeof(EmaSmoothingFilter.Config), typeDiscriminator: nameof(EmaSmoothingFilter))]
[JsonDerivedType(typeof(MaxSpikeFilter.Config), typeDiscriminator: nameof(MaxSpikeFilter))]
[JsonDerivedType(typeof(MedianSmoothingFilter.Config), typeDiscriminator: nameof(MedianSmoothingFilter))]
[JsonDerivedType(typeof(RoundingPrecisionFilter.Config), typeDiscriminator: nameof(RoundingPrecisionFilter))]
[JsonDerivedType(typeof(PinLocalMaximaFilter.Config), typeDiscriminator: nameof(PinLocalMaximaFilter))]
[JsonDerivedType(typeof(SimplifyFilter.Config), typeDiscriminator: nameof(SimplifyFilter))]
public interface IPointFilterConfig : IFilterConfig
{
    IFilter<MutableGpxPoint> CreateFilter();
}

// Configs of filters that work on candidate peaks (peak matching only).
[JsonPolymorphic(TypeDiscriminatorPropertyName = "FilterType")]
[JsonDerivedType(typeof(MergeNearbyByDistanceFilter.Config), typeDiscriminator: nameof(MergeNearbyByDistanceFilter))]
[JsonDerivedType(typeof(MergeNearbyByElevationFilter.Config), typeDiscriminator: nameof(MergeNearbyByElevationFilter))]
[JsonDerivedType(typeof(PeakLookupFilter.Config), typeDiscriminator: nameof(PeakLookupFilter))]
[JsonDerivedType(typeof(MatchWithinDistanceFilter.Config), typeDiscriminator: nameof(MatchWithinDistanceFilter))]
[JsonDerivedType(typeof(LocalMaximaFilter.Config), typeDiscriminator: nameof(LocalMaximaFilter))]
public interface IPeakFilterConfig : IFilterConfig
{
    IPeakFilter CreateFilter(PeakFilterContext context);
}

public interface ITransform<TIn, TOut>
{
    IList<TOut> Apply(IList<TIn> values);
}