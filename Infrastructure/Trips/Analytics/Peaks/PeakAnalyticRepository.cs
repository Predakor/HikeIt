using Domain.Trips.Analytics.Peaks;
using Infrastructure.Commons.Databases;
using Infrastructure.Commons.Repositories;

namespace Infrastructure.Trips.Analytics.Peaks;

public class PeakAnalyticRepository
    : CrudResultRepository<PeaksAnalytic, Guid>,
        IPeakAnalyticRepository {
    public PeakAnalyticRepository(TripDbContext context)
        : base(context) { }
}
