using Application.Dto;
using Domain.Trips.ValueObjects;

namespace Application.Services.Peaks;

public interface IPeakService {
    Task<IEnumerable<PeakDto.Complete>> GetAllPeaksAsync();
    Task<PeakDto.Complete> GetPeakByIdAsync(int id);
    Task<List<PeakDto.Reached>> GetMatchingPeaks(IEnumerable<IGeoPoint> points);
}
