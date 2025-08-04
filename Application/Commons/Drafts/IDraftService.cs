namespace Application.Commons.Drafts;

public interface IDraftService<T>
    where T : class, IDraft {
    Task<T> Add(T draft);
    Task Remove(Guid id);
    Task<T> Get(Guid id);
}
