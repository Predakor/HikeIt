using Application.Dto.Analytics;
using Application.TripAnalytics.Quries;
using Domain.Common;
using Domain.Common.Result;
using Domain.TripAnalytics;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static Application.Dto.Analytics.TripAnalyticsDto;

namespace Infrastructure.TripAnalytics.Queries;

public class TripAnalyticsQueryService(TripDbContext context) : ITripAnalyticsQueryService {
    readonly TripDbContext _context = context;

    public async Task<Result<TripAnalyticsDto.Full>> GetCompleteAnalytics(Guid id) {
        var analytics = await _context
            .TripAnalytics.AsNoTracking()
            .Include(a => a.PeaksAnalytic)
            .Include(a => a.ElevationProfile)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (analytics == null) {
            return Errors.NotFound($"analytics with id: {id}");
        }

        if (analytics.PeaksAnalytic != null) {
            var reachedPeaks = _context
                .ReachedPeaks.AsNoTracking()
                .Where(peak => peak.TripId == id)
                .Include(p => p.Peak);
            analytics.PeaksAnalytic.ReachedPeaks = [.. reachedPeaks];
        }

        return MapToDto(analytics);
    }

    public async Task<Result<TripAnalyticsDto.Basic>> GetBasicAnalytics(Guid tripId) {
        var basePath = $"/api/trips/{tripId}/analytics";

        var dto = await _context
            .TripAnalytics.AsNoTracking()
            .Where(t => t.Id == tripId)
            .Select(t => new TripAnalyticsDto.Basic(
                t.RouteAnalytics,
                t.TimeAnalytics,
                t.PeaksAnalytic != null ? new Uri($"{basePath}/peaks", UriKind.Relative) : null,
                t.ElevationProfile != null
                    ? new Uri($"{basePath}/elevation", UriKind.Relative)
                    : null
            ))
            .FirstOrDefaultAsync();

        if (dto is null) {
            return Errors.NotFound($"analytics with id:{tripId}");
        }

        return dto;
    }

    public Task<Result<PeakAnalyticsDto>> GetPeakAnalytics(Guid id) {
        throw new NotImplementedException();
    }

    static Full MapToDto(TripAnalytic data) {
        var elevationDto = data.ElevationProfile?.ToDto();
        var peakAnalyticDto = data.PeaksAnalytic?.ToDto();
        var timeAnalyticDto = data.TimeAnalytics ?? null;
        var routeAnalyticsDto = data.RouteAnalytics ?? null;

        return new Full(routeAnalyticsDto, timeAnalyticDto, peakAnalyticDto, elevationDto, data.Id);
    }
}
