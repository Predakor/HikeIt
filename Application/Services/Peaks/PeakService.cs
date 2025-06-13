using Application.Dto;
using Application.Mappers.Implementations;
using Domain.Trips.ValueObjects;

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

    public async Task<List<PeakDto.Reached>> GetMatchingPeaks(IEnumerable<IGeoPoint> points) {
        //TOIMPlemtn
        await Task.Delay(1);
        return [new(1, 1065)];
    }
}
