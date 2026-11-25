namespace Application.Commons.Abstractions;

/// <summary>Marks a decorator that caches results of TService; it is found and applied by DependencyInjection.</summary>
public interface ICacheable<TService> : IDecorator<TService>
    where TService : class;
