using Application.Dto;
using Application.Mappers.Implementations;
using Domain.Entiites.Peaks;
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

    public async Task CreatePeakAsync(PeakDto.Simple peakDto) {
        var newPeak = _peakMapper.MapToEntity(peakDto);
        await _repo.AddAsync(newPeak);
    }

    public async Task<List<PeakDto.Reached>> GetMatchingPeaks(List<GpxPoint> points) {
        //TOIMPlemtn
        await Task.Delay(1);
        return [new(1)];
        throw new NotImplementedException();
    }

    public async Task<List<PeakDto.Reached>> GetMatchingPeaks(List<GpxPointWithTime> points) {
        //TOIMPlemtn
        await Task.Delay(1);
        return [new(1, DateTime.Now)];
        throw new NotImplementedException();
    }
}
