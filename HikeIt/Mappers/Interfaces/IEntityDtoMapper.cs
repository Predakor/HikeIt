namespace HikeIt.Api.Mappers.Interfaces;

public interface IEntityDtoMapper<TEntity, TDto>
    where TDto : class
    where TEntity : class {
    public TDto MapToDto(TEntity entity);
    public TEntity MapToEntity(TDto dto);
}