using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public abstract class Repository<T>(TripDbContext context)
    where T : class, IEntity {
    protected readonly TripDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();


    public virtual async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id).AsTask();

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();


    public virtual Task<int> SaveChangesAsync() => Context.SaveChangesAsync();

}
