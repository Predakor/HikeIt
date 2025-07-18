using Application.Dto;
using Application.Mappers.Implementations;
using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Entiites.Regions;

namespace Application.Services.Regions;

public class RegionService(IRegionRepository repository) : IRegionService {
    readonly IRegionRepository _repository = repository;

    public async Task<Result<RegionDto>> GetAsync(int id) {
        return await _repository.GetByIdAsync(id).MapAsync(RegionMapper.MapToDto);
    }

    public async Task<Result<List<RegionDto.Complete>>> GetAllAsync() {
        return await _repository.GetAllAsync().MapAsync(Helpers.MapToComplete);
    }

    public async Task<Result<RegionDto.WithPeaks>> GetAllFromRegion(Region region) {
        return await _repository.AllPeaksFromRegion(region)
            .MapAsync(Helpers.MapToPeaksWithLocations)
            .MapAsync(peaks => Helpers.MapToRegionWithPeaks(region, peaks));
    }

    internal static class Helpers {
        public static RegionDto.WithPeaks MapToRegionWithPeaks(Region region, List<PeakDto.WithLocation> peaks) {
            return new RegionDto.WithPeaks(RegionMapper.MapToCompleteDto(region), peaks);
        }
        public static List<PeakDto.WithLocation> MapToPeaksWithLocations(IEnumerable<Peak> peaks) {
            return [.. peaks.Select(PeakMapper.ToWithLocation)];
        }

        public static List<RegionDto.Complete> MapToComplete(IEnumerable<Region> regions) {
            return [.. regions.Select(RegionMapper.MapToCompleteDto).ToList()];
        }
    }
}
