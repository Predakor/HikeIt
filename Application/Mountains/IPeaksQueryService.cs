using Application.Dto;
using Application.Interfaces;
using Domain.Common.Result;
using Domain.Mountains.Peaks;
using Domain.Trips.ValueObjects;

namespace Application.Mountains;

public interface IPeaksQueryService : IQueryService {
    Task<Result<PeakDto.Complete>> GetByIdAsync(int id);
    Task<Result<IEnumerable<PeakDto.Complete>>> GetAllAsync();
    Task<Result<Peak>> GetPeakWithinRadius(GpxPoint point, float radius);
    Task<Result<List<Peak>>> GetPeaksWithinRadius(IEnumerable<GpxPoint> points, float radius);
}
