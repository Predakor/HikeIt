namespace Core.Abstractions;

/// <summary>Marks a service interface for scoped registration under its own interfaces by the Api service scan.</summary>
public interface IService;

/// <summary>Marks a service interface for the same scan with a singleton lifetime.</summary>
public interface ISingletonService;
