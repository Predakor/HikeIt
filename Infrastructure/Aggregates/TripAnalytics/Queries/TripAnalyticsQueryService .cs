using Application.Dto.Analytics;
using Application.TripAnalytics.Quries;
using Domain.Common;
using Domain.Common.Result;
using Domain.TripAnalytics;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static Application.Dto.Analytics.TripAnalyticsDto;

namespace Infrastructure.Aggregates.TripAnalytics.Queries;

public class TripAnalyticsQueryService : ITripAnalyticsQueryService {
    readonly TripDbContext _context;

    public TripAnalyticsQueryService(TripDbContext context) {
        _context = context;
    }

    IQueryable<TripAnalytic> Analytics => _context.TripAnalytics.AsNoTracking();

    public async Task<Result<Full>> GetCompleteAnalytics(Guid id) {
        var analytics = await Analytics
            .Include(a => a.PeaksAnalytic)
            .Include(a => a.ElevationProfile)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (analytics == null) {
            return Errors.NotFound($"analytics with id: {id}");
        }

        var peakAnalytics = analytics.PeaksAnalytic is not null ? await GetPeakAnalytics(id) : null;

        if (analytics.PeaksAnalytic != null) { }

        return MapToDto(analytics, peakAnalytics?.Value);
    }

    public async Task<Result<Basic>> GetBasicAnalytics(Guid tripId) {
        var basePath = $"/api/trips/{tripId}/analytics";

        var dto = await Analytics
            .Where(t => t.Id == tripId)
            .Select(t => t.ToBasicDto())
            .FirstOrDefaultAsync();

        if (dto is null) {
            return Errors.NotFound($"analytics with id:{tripId}");
        }

        return dto;
    }

    public async Task<Result<PeakAnalyticsDto>> GetPeakAnalytics(Guid id) {
        var analytics = await _context
            .PeaksAnalytics.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (analytics is null) {
            return Errors.NotFound("peak analytics", "id", id);
        }

        var peaks = await _context
            .ReachedPeaks.AsNoTracking()
            .Where(rp => rp.TripId == id)
            .Include(p => p.Peak)
            .ToListAsync();

        return analytics.ToDto(peaks);
    }

    static Full MapToDto(TripAnalytic data, PeakAnalyticsDto? peakAnalyticsDto) {
        var elevationDto = data.ElevationProfile?.ToDto();
        var peakAnalyticDto = peakAnalyticsDto ?? null;
        var timeAnalyticDto = data.TimeAnalytics ?? null;
        var routeAnalyticsDto = data.RouteAnalytics ?? null;

        return new Full(routeAnalyticsDto, timeAnalyticDto, peakAnalyticDto, elevationDto, data.Id);
    }



}
