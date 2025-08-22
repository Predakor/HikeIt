using Application.Commons.Abstractions.Queries;
using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.PeakAnalytics;
using Domain.Common.Result;

namespace Application.Trips.Analytics.Queries;

public interface ITripAnalyticsQueryService : IQueryService {
    Task<Result<TripAnalyticsDto.Basic>> GetBasicAnalytics(Guid tripId);
    Task<Result<TripAnalyticsDto.Full>> GetCompleteAnalytics(Guid id);
    Task<Result<PeakAnalyticsDto>> GetPeakAnalytics(Guid id);
}
