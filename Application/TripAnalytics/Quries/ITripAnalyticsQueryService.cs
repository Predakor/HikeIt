using Application.Dto.Analytics;
using Domain.Common.Result;

namespace Application.TripAnalytics.Quries;

public interface ITripAnalyticsQueryService {
    Task<Result<TripAnalyticsDto.Basic>> GetBasicAnalytics(Guid tripId);
    Task<Result<TripAnalyticsDto.Full>> GetCompleteAnalytics(Guid id);
    Task<Result<PeakAnalyticsDto>> GetPeakAnalytics(Guid id);

}
