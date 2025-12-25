namespace Domain.Common.Abstractions;
public interface ISoftDeletable<TEntity> where TEntity : IEntity
{
    DateTimeOffset? DeletedAt { get; }
    bool IsDeleted { get; }
    TEntity Delete(DateTimeOffset time);
}