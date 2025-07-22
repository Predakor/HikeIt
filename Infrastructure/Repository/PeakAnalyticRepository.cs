using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.TripAnalytics.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository;

public class PeakAnalyticRepository
    : CrudResultRepository<PeaksAnalytic, Guid>,
        IPeakAnalyticRepository {
    public PeakAnalyticRepository(TripDbContext context)
        : base(context) { }
}
