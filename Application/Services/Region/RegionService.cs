using Application.Dto;
using Application.Mappers.Implementations;
using Domain.Regions;

namespace Application.Services.Region;

public interface IRegionService {
    public Task<RegionDto> GetAsync(int id);
    public Task<List<RegionDto.Complete>> GetAllAsync();
}
public class RegionService(RegionMapper mapper, IRegionRepository repository) : IRegionService {

    readonly RegionMapper _mapper = mapper;
    readonly IRegionRepository _repository = repository;

    public async Task<RegionDto> GetAsync(int id) {
        var result = await _repository.GetAsync(id);
        if (result == null) {
            return null;
        }
        return _mapper.MapToDto(result);
    }

    public async Task<List<RegionDto.Complete>> GetAllAsync() {
        var result = await _repository.GetAllAsync();

        if (!result.Any()) {
            return null;
        }

        var mappedResults = result.Select(_mapper.MapToCompleteDto).ToList();
        return mappedResults;
    }
}
