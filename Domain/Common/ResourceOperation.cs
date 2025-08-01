namespace Domain.Common;

public interface IResourceOperation { }

public record ResourceOperation<TResourceKey, TResourceData>(
    Guid UserId,
    TResourceKey ResourceKey,
    TResourceData ResourceData
) : IResourceOperation;
