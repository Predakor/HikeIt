using Domain.Regions;
using Infrastructure.Data;

namespace Infrastructure.Repository;


public class RegionRepository : Repository<Region>, IRegionRepository {
    public RegionRepository(TripDbContext context) : base(context) { }
}
