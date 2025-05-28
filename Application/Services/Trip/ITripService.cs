using Application.Dto;

namespace Application.Services.Trip;

public interface ITripService {
    public Task<bool> Add(TripDto.CompleteLinked dto);
    public Task<bool> Update(TripDto.UpdateDto dto);
    public Task<bool> Delete(int id);

    public Task<TripDto.Complete?> GetById(int id);
    public Task<List<TripDto.CompleteLinked>> GetAll();
}
