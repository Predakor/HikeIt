using Domain.Common.Abstractions;

namespace Domain.Trips.Analytics.Peaks;
public interface IPeakAnalyticRepository : ICrudResultRepository<PeaksAnalytic, Guid> {
}
