using Application.Services.Peaks;
using Domain.Entiites.Peaks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class PeakRepository : Repository<Peak, int>, IPeakRepository {
    public PeakRepository(TripDbContext context) : base(context) { }

    public override async Task<IEnumerable<Peak>> GetAllAsync() {
        return await DbSet.Include(x => x.Region).ToListAsync();
    }

    public override async Task<Peak?> GetByIdAsync(int id) {
        return await DbSet.Include(x => x.Region).FirstOrDefaultAsync(e => e.Id == id);
    }
}