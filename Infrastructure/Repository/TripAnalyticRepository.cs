using Domain.TripAnalytics;
using Domain.TripAnalytics.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository;

public class TripAnalyticRepository : CrudRepository<TripAnalytic, Guid>, ITripAnalyticRepository {
    public TripAnalyticRepository(TripDbContext context)
        : base(context) { }
}
