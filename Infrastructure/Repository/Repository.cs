using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public abstract class Repository<T>(TripDbContext context)
    where T : class, IRepositoryObject {
    protected readonly TripDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();


    public virtual async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id).AsTask();

    public virtual async Task<List<T>> GetAllAsync() => await DbSet.ToListAsync();

    public virtual async Task AddAsync(T entity) => await DbSet.AddAsync(entity).AsTask();

    public virtual void Remove(T entity) => DbSet.Remove(entity);
    public virtual Task<int> SaveChangesAsync() => Context.SaveChangesAsync();

}
