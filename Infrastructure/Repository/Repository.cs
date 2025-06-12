using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public abstract class BaseRepository<T, TKey> : IRepository<T, TKey>
    where T : class, IEntity<TKey> {
    protected readonly TripDbContext Context;
    protected readonly DbSet<T> DbSet;

    public BaseRepository(TripDbContext context) {
        Context = context;
        DbSet = context.Set<T>();
    }
}

public abstract class Repository<T, TKey> : BaseRepository<T, TKey>
    where T : class, IEntity<TKey> {
    protected Repository(TripDbContext context) : base(context) { }

    public virtual async Task<T?> GetByIdAsync(TKey id) => await DbSet.FindAsync(id).AsTask();

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();

    public virtual async Task<bool> SaveChangesAsync() {
        try {
            return await Context.SaveChangesAsync() > 0;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }
}
