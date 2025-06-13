using Application.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository;

public class PeakAnalyticRepository
    : CrudResultRepository<PeaksAnalytic, Guid>,
        IPeakAnalyticRepository {
    public PeakAnalyticRepository(TripDbContext context)
        : base(context) { }
}
