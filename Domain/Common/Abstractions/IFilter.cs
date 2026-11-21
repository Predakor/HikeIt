using Domain.Trips.Analytics.Route.Filters;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Analytics.Time.Filters;
using Domain.Trips.Root.Builders.GpxDataBuilder;
using System.Text.Json.Serialization;

namespace Domain.Common.Abstractions;

// One step of a chain. Filters of a family share a state type and refine it in order.
public interface IFilter<TState>
{
    ValueTask<TState> ApplyAsync(TState state);
}

// Base for filters that never await.
public abstract class SyncFilter<TState> : IFilter<TState>
{
    public abstract TState Apply(TState state);

    public ValueTask<TState> ApplyAsync(TState state) => ValueTask.FromResult(Apply(state));
}

public interface IFilterConfig<TState> : IFilterConfig
{
    IFilter<TState> CreateFilter();
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "filterType")]
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
[JsonDerivedType(typeof(IdleFilter.Config), typeDiscriminator: nameof(IdleFilter))]
[JsonDerivedType(typeof(GradeFilter.Config), typeDiscriminator: nameof(GradeFilter))]
[JsonDerivedType(typeof(SpeedSegmentFilter.Config), typeDiscriminator: nameof(SpeedSegmentFilter))]
[JsonDerivedType(typeof(ElevationGainFilter.Config), typeDiscriminator: nameof(ElevationGainFilter))]
[JsonDerivedType(typeof(SteepestSegmentFilter.Config), typeDiscriminator: nameof(SteepestSegmentFilter))]
[JsonDerivedType(typeof(LongestClimbFilter.Config), typeDiscriminator: nameof(LongestClimbFilter))]
public interface IFilterConfig;

// Configs of filters that work on track points (cleanup, elevation profile, route).
[JsonPolymorphic(TypeDiscriminatorPropertyName = "filterType")]
[JsonDerivedType(typeof(EmaSmoothingFilter.Config), typeDiscriminator: nameof(EmaSmoothingFilter))]
[JsonDerivedType(typeof(MaxSpikeFilter.Config), typeDiscriminator: nameof(MaxSpikeFilter))]
[JsonDerivedType(typeof(MedianSmoothingFilter.Config), typeDiscriminator: nameof(MedianSmoothingFilter))]
[JsonDerivedType(typeof(RoundingPrecisionFilter.Config), typeDiscriminator: nameof(RoundingPrecisionFilter))]
[JsonDerivedType(typeof(PinLocalMaximaFilter.Config), typeDiscriminator: nameof(PinLocalMaximaFilter))]
[JsonDerivedType(typeof(SimplifyFilter.Config), typeDiscriminator: nameof(SimplifyFilter))]
public interface IPointFilterConfig : IFilterConfig<IList<MutableGpxPoint>>;

// Configs of filters that work on candidate peaks (peak matching only).
[JsonPolymorphic(TypeDiscriminatorPropertyName = "filterType")]
[JsonDerivedType(typeof(MergeNearbyByDistanceFilter.Config), typeDiscriminator: nameof(MergeNearbyByDistanceFilter))]
[JsonDerivedType(typeof(MergeNearbyByElevationFilter.Config), typeDiscriminator: nameof(MergeNearbyByElevationFilter))]
[JsonDerivedType(typeof(PeakLookupFilter.Config), typeDiscriminator: nameof(PeakLookupFilter))]
[JsonDerivedType(typeof(MatchWithinDistanceFilter.Config), typeDiscriminator: nameof(MatchWithinDistanceFilter))]
[JsonDerivedType(typeof(LocalMaximaFilter.Config), typeDiscriminator: nameof(LocalMaximaFilter))]
public interface IPeakFilterConfig : IFilterConfig<PeakChain>;

// Configs of filters that classify and measure time segments (time analytics only).
[JsonPolymorphic(TypeDiscriminatorPropertyName = "filterType")]
[JsonDerivedType(typeof(IdleFilter.Config), typeDiscriminator: nameof(IdleFilter))]
[JsonDerivedType(typeof(GradeFilter.Config), typeDiscriminator: nameof(GradeFilter))]
[JsonDerivedType(typeof(SpeedSegmentFilter.Config), typeDiscriminator: nameof(SpeedSegmentFilter))]
public interface IPaceFilterConfig : IFilterConfig<TimeAnalyticContext>;

// Configs of filters that fill in the route analytics (route analytics only).
[JsonPolymorphic(TypeDiscriminatorPropertyName = "filterType")]
[JsonDerivedType(typeof(ElevationGainFilter.Config), typeDiscriminator: nameof(ElevationGainFilter))]
[JsonDerivedType(typeof(SteepestSegmentFilter.Config), typeDiscriminator: nameof(SteepestSegmentFilter))]
[JsonDerivedType(typeof(LongestClimbFilter.Config), typeDiscriminator: nameof(LongestClimbFilter))]
public interface ITerrainFilterConfig : IFilterConfig<RouteAnalyticContext>;

public interface ITransform<TIn, TOut>
{
    IList<TOut> Apply(IList<TIn> values);
}