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

public interface IFilterConfig
{
    string Name { get; }
    Type FilterType { get; }
    object GetValue();
}

public interface IFilterConfig<out T> : IFilterConfig
{
    T Value { get; }
}

public interface ITransform<TIn, TOut>
{
    IList<TOut> Apply(IList<TIn> values);
}
