using Domain.Common.Result;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.TripAnalytics.Interfaces;
public interface IPeakAnalyticService {
    Task<Result<PeaksAnalytic>> Create(IEnumerable<ReachedPeak> Peaks);
    Task<Result<PeaksAnalytic>> GeneratePeakAnalytic(IEnumerable<ReachedPeak> Peaks);
}