using Application.Dto;
using Domain.Common;

namespace Application.Services.Trip;

public interface ITripService {
    public Task<Result<Guid>> Add(TripDto.Request.Create dto);


    public Task<bool> Update(TripDto.Request.Update dto);
    public Task<bool> UpdateGpxFile(Guid id, Guid gpxFileId);
    public Task<bool> Delete(Guid id);

    public Task<TripDto.Partial?> GetById(Guid id);
    public Task<List<TripDto.Request.ResponseBasic>> GetAll();
}
