using Domain.Common.Abstractions;
using Infrastructure.Commons.Databases;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commons.Repositories;

public abstract class BaseRepository<T, TKey> : IRepository<T, TKey>
    where T : class, IEntity<TKey> {
    protected readonly TripDbContext Context;
    protected readonly DbSet<T> DbSet;

    public BaseRepository(TripDbContext context) {
        Context = context;
        DbSet = context.Set<T>();
    }
}
