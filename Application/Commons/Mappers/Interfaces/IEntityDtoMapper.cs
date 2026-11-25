namespace Application.Commons.Mappers.Interfaces;

public interface IEntityDtoMapper<in TEntity, out TDto>
    where TDto : class
    where TEntity : class
{
    TDto MapToDto(TEntity entity);
}
