using Domain.Regions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;


public class RegionRepository : Repository<Region>, IRegionRepository {
    public RegionRepository(TripDbContext context) : base(context) { }

    public async Task<Region?> GetAsync(int id) {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<Region>> GetAllAsync() {
        return await DbSet.ToListAsync();
    }
}
