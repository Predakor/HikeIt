namespace HikeIt.Api.Repository;

public class MemoryRepository<T> : IRepository<T>
    where T : class, IRepositoryObject {
    private readonly List<T> _items = new();

    public async Task AddAsync(T entity) {
        _items.Add(entity);
        Console.WriteLine($"Added entity: {entity}"); // Debug output
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<T>> GetAllAsync() {
        return await Task.FromResult(_items);
    }

    public async Task<T?> GetByIDAsync(int id) {
        var entity = _items.FirstOrDefault(e => (e as dynamic).Id == id); // Assuming T has Id property
        return await Task.FromResult(entity);
    }

    public async Task RemoveAsync(int id) {
        var entity = _items.FirstOrDefault(e => (e as dynamic).Id == id);
        if (entity is not null) {
            _items.Remove(entity);
        }
        await Task.CompletedTask;
    }

    public async Task<bool> UpdateAsync(int id, T updatedEntity) {
        var entity = _items.FirstOrDefault(e => (e as dynamic).Id == id);
        if (entity is not null) {
            _items.Remove(entity);
            _items.Add(updatedEntity);
            return true;

        }
        await Task.CompletedTask;
        return false;
    }
}
