using Application.Commons.Abstractions.Queries;
using Domain.Common.Geography.ValueObjects;
using Domain.Locations.Peaks;
using Domain.ReachedPeaks.Builders;

namespace Application.Locations.Peaks;

public interface IPeaksQueryService : IQueryService {
    Task<Result<PeakDto.Complete>> GetByIdAsync(int id);
    Task<Result<IEnumerable<PeakDto.Complete>>> GetAllAsync();
    Task<Result<Peak>> GetPeakWithinRadius(GpxPoint point, float radius);
    Task<Result<Peak>> GetPeakWithNameFromRegion(string name, int regionId);
    Task<Result<List<Peak>>> GetPeaksWithinRadius(IEnumerable<GpxPoint> points, float radius);
    Task<Result<List<ReachedPeakDataBuilder>>> GetPeaksWithinRadius(
        IEnumerable<ReachedPeakDataBuilder> points,
        float radius
    );
}
