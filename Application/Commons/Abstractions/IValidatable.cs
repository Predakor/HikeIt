namespace Application.Commons.Abstractions;

/// <summary>Marks a decorator that validates calls to TService; it is found and applied by DependencyInjection.</summary>
public interface IValidatable<TService> : IDecorator<TService>
    where TService : class;
