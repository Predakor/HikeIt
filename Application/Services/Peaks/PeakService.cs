using Application.Dto;
using Application.Mappers.Implementations;
using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Trips.ValueObjects;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Application.Services.Peaks;

public class PeakService(IPeakRepository repo, PeakMapper peakMapper) : IPeakService {
    readonly IPeakRepository _repo = repo;
    readonly PeakMapper _peakMapper = peakMapper;

    public async Task<IEnumerable<PeakDto.Complete>> GetAllPeaksAsync() {
        var peaks = await _repo.GetAllAsync();
        return peaks.Select(_peakMapper.MapToCompleteDto);
    }

    public async Task<PeakDto.Complete?> GetPeakByIdAsync(int id) {
        var peak = await _repo.GetByIdAsync(id);
        if (peak == null)
            return null;
        return _peakMapper.MapToCompleteDto(peak);
    }

    public async Task<Result<List<PeakDto.Reached>>> GetMatchingPeaks(IEnumerable<GpxPoint> points) {
        //TOIMPlemtn
        await Task.Delay(1);
        var reachedPeak = new PeakDto.Reached(1, 1605);
        var list = new List<PeakDto.Reached> { reachedPeak };

        return list;
    }

    public async Task<Result<Peak>> GetPeakWithinRadius(GpxPoint point, float radius) {
        return await _repo.GetPeakWithinRadius(point.ToGpxPoint(), radius);
    }

    public async Task<Result<IList<Peak>>> GetPeaksWithinRadius(
        IEnumerable<GpxPoint> points,
        float radius
    ) {
        return await _repo.GetPeaksWithinRadius(points.ToGpxPoints(), radius);
    }
}

internal static class PeakExtentions {
    public static Point ToGpxPoint(this GpxPoint point) {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var gpxLocation = geometryFactory.CreatePoint(new Coordinate(point.Lat, point.Lon));
        return gpxLocation;
    }

    public static IReadOnlyList<Point> ToGpxPoints(this IEnumerable<GpxPoint> points) {
        return points.Select(p => p.ToGpxPoint()).ToList();
    }
}
