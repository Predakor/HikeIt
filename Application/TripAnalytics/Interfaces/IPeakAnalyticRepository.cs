using Domain.Interfaces;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.TripAnalytics.Interfaces;
public interface IPeakAnalyticRepository : ICrudResultRepository<PeaksAnalytic, Guid> {
}
