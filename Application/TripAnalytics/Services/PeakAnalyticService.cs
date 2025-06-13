using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.TripAnalytics.Services;

public record PeakAnalyticData(List<ReachedPeak> Peaks);

public class PeakAnalyticService(IPeakAnalyticRepository repository) : IPeakAnalyticService {
    readonly IPeakAnalyticRepository _repository = repository;

    public async Task<Result<PeaksAnalytic>> Create(PeakAnalyticData data) {
        var analytics = PeaksAnalytic.Create(data.Peaks);
        return await _repository.AddAsync(analytics);
    }
}
