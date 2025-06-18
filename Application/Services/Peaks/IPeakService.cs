using Application.Dto;
using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Trips.ValueObjects;

namespace Application.Services.Peaks;

public interface IPeakService {
    Task<PeakDto.Complete> GetPeakByIdAsync(int id);
    Task<IEnumerable<PeakDto.Complete>> GetAllPeaksAsync();


    Task<Result<Peak>> GetPeakWithinRadius(GpxPoint point, float radius);
    Task<Result<IList<Peak>>> GetPeaksWithinRadius(IEnumerable<GpxPoint> points, float radius);

    Task<Result<List<PeakDto.Reached>>> GetMatchingPeaks(IEnumerable<GpxPoint> points);


}
