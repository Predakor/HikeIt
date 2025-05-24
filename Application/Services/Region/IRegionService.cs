using Application.Dto;

namespace Application.Services.Region;

public interface IRegionService {
    public Task<RegionDto> GetAsync(int id);
    public Task<List<RegionDto.Complete>> GetAllAsync();
}
