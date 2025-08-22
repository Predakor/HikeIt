using Domain.Common.Abstractions;
using Infrastructure.Commons.Databases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commons.Repositories;

[Obsolete("User result repository instead")]
public abstract class Repository<T, TKey> : BaseRepository<T, TKey>
    where T : class, IEntity<TKey> {
    protected Repository(TripDbContext context)
        : base(context) { }

    public virtual async Task<T?> GetByIdAsync(TKey id) => await DbSet.FindAsync(id).AsTask();

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();

    public virtual async Task<bool> SaveChangesAsync() {
        try {
            return await Context.SaveChangesAsync() > 0;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            return false;
        }
    }
}

[Obsolete("User result repository instead")]
public abstract class CrudRepository<T, TKey> : Repository<T, TKey>, ICrudRepository<T, TKey>
    where T : class, IEntity<TKey> {
    protected CrudRepository(TripDbContext context)
        : base(context) { }

    public async Task<bool> AddAsync(T entity) {
        return await DbSet.AddAsync(entity) != null;
    }
    public T Add(T entity) {
        DbSet.Add(entity);
        return entity;
    }

    public Task<bool> RemoveAsync(TKey id) {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(TKey id) {
        throw new NotImplementedException();
    }
}
