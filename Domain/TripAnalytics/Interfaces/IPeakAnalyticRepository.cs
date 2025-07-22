using Domain.Interfaces;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Domain.TripAnalytics.Interfaces;
public interface IPeakAnalyticRepository : ICrudResultRepository<PeaksAnalytic, Guid> {
}
