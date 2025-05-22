using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class SqlRepository<T>(TripDbContext context) : IRepository<T>
    where T : class, IRepositoryObject {
    private readonly TripDbContext _context = context;

    public async Task AddAsync(T entity) {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync() {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIDAsync(int id) {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task RemoveAsync(int id) {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity is not null) {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> UpdateAsync(int id, T updatedEntity) {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity is not null) {
            _context.Entry(entity).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
