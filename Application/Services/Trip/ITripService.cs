using Application.Dto;

namespace Application.Services.Trip;

public interface ITripService {
    public Task<bool> Add(TripDto.Request.Create dto);
    public Task<bool> Update(TripDto.Request.Update dto);
    public Task<bool> Delete(int id);

    public Task<TripDto.Partial?> GetById(int id);
    public Task<List<TripDto.Request.ResponseBasic>> GetAll();
}
