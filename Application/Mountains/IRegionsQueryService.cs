using Application.Dto;
using Domain.Common.Result;

namespace Application.Mountains;

public interface IRegionQueryService {
    Task<Result<RegionDto.Complete>> GetById(int id);
    Task<Result<RegionDto.Complete>> GetByName(string name);
    Task<Result<List<RegionDto.Complete>>> GetAllAsync();
    Task<Result<RegionDto.WithPeaks>> AllPeaksFromRegion(RegionDto.Complete region);
}
