using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public abstract class Repository<T, TKey>(TripDbContext context)
    where T : class, IEntity<TKey> {
    protected readonly TripDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();


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
