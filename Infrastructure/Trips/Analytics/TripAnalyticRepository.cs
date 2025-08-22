using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Root.Interfaces;
using Infrastructure.Commons.Databases;
using Infrastructure.Commons.Repositories;

namespace Infrastructure.Trips.Analytics;

public class TripAnalyticRepository : CrudRepository<TripAnalytic, Guid>, ITripAnalyticRepository {
    public TripAnalyticRepository(TripDbContext context)
        : base(context) { }
}
