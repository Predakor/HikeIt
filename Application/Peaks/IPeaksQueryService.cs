using Application.Dto;
using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Trips.ValueObjects;

namespace Application.Peaks;

public interface IPeaksQueryService {
    Task<Result<PeakDto.Complete>> GetByIdAsync(int id);
    Task<Result<IEnumerable<PeakDto.Complete>>> GetAllAsync();
    Task<Result<Peak>> GetPeakWithinRadius(GpxPoint point, float radius);
    Task<Result<List<Peak>>> GetPeaksWithinRadius(IEnumerable<GpxPoint> points, float radius);
}
