using Application.TripAnalytics.Services;
using Domain.Common;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.TripAnalytics.Interfaces;
public interface IPeakAnalyticService {
    Task<Result<PeaksAnalytic>> Create(PeakAnalyticData data);
}