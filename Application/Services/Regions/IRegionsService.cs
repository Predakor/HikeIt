using Application.Dto;
using Domain.Common.Result;
using Domain.Entiites.Regions;

namespace Application.Services.Regions;

public interface IRegionService {
    Task<Result<RegionDto>> GetAsync(int id);
    Task<Result<List<RegionDto.Complete>>> GetAllAsync();
    Task<Result<RegionDto.WithPeaks>> GetAllFromRegion(Region region);
}
