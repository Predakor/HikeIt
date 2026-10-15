using Domain.Trips.Analytics.Shared.Filters;
using System.Text.Json.Serialization;

namespace Application.Commons.Abstractions;

public interface IFilter<TValue>
{
    IList<TValue> Apply(IList<TValue> values);
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(FilterType))]
[JsonDerivedType(typeof(EmaSmoothingFilter.Config), typeDiscriminator: nameof(EmaSmoothingFilter))]
[JsonDerivedType(typeof(MaxSpikeFilter.Config), typeDiscriminator: nameof(MaxSpikeFilter))]
[JsonDerivedType(typeof(MedianSmoothingFilter.Config), typeDiscriminator: nameof(MedianSmoothingFilter))]
[JsonDerivedType(typeof(RoundingPrecisionFilter.Config), typeDiscriminator: nameof(RoundingPrecisionFilter))]
[JsonDerivedType(typeof(PinLocalMaximaFilter.Config), typeDiscriminator: nameof(PinLocalMaximaFilter))]
[JsonDerivedType(typeof(SimplifyFilter.Config), typeDiscriminator: nameof(SimplifyFilter))]
[JsonDerivedType(typeof(MergeNearbyByDistanceFilter.Config), typeDiscriminator: nameof(MergeNearbyByDistanceFilter))]
[JsonDerivedType(typeof(MergeNearbyByElevationFilter.Config), typeDiscriminator: nameof(MergeNearbyByElevationFilter))]
[JsonDerivedType(typeof(CoordinateSmoothingFilter.Config), typeDiscriminator: nameof(CoordinateSmoothingFilter))]
public interface IFilterConfig
{
    Type FilterType { get; }
    object GetValue();
}

public interface IFilterConfig<out T> : IFilterConfig
{
    T Value { get; }
}

// Configs of filters that work on track points (cleanup, elevation profile, route).
[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(FilterType))]
[JsonDerivedType(typeof(EmaSmoothingFilter.Config), typeDiscriminator: nameof(EmaSmoothingFilter))]
[JsonDerivedType(typeof(MaxSpikeFilter.Config), typeDiscriminator: nameof(MaxSpikeFilter))]
[JsonDerivedType(typeof(MedianSmoothingFilter.Config), typeDiscriminator: nameof(MedianSmoothingFilter))]
[JsonDerivedType(typeof(RoundingPrecisionFilter.Config), typeDiscriminator: nameof(RoundingPrecisionFilter))]
[JsonDerivedType(typeof(PinLocalMaximaFilter.Config), typeDiscriminator: nameof(PinLocalMaximaFilter))]
[JsonDerivedType(typeof(SimplifyFilter.Config), typeDiscriminator: nameof(SimplifyFilter))]
[JsonDerivedType(typeof(CoordinateSmoothingFilter.Config), typeDiscriminator: nameof(CoordinateSmoothingFilter))]
public interface IPointFilterConfig : IFilterConfig;

// Configs of filters that work on candidate peaks (peak matching only).
[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(FilterType))]
[JsonDerivedType(typeof(MergeNearbyByDistanceFilter.Config), typeDiscriminator: nameof(MergeNearbyByDistanceFilter))]
[JsonDerivedType(typeof(MergeNearbyByElevationFilter.Config), typeDiscriminator: nameof(MergeNearbyByElevationFilter))]
public interface IPeakFilterConfig : IFilterConfig;

public interface ITransform<TIn, TOut>
{
    IList<TOut> Apply(IList<TIn> values);
}
