using Domain.Entiites.Regions;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository;


public class RegionRepository : Repository<Region, int>, IRegionRepository {
    public RegionRepository(TripDbContext context) : base(context) { }
}
