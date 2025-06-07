using Application.Dto;
using Domain.Common;
using Domain.Trips.ValueObjects;

namespace Application.Services.Trips;

public interface ITripService {
    public Task<Result<Guid>> Add(TripDto.Request.Create dto);
    public Task<Result<Guid>> Add(TripDto.Request.Create dto, TripAnalyticData data);

    public Task<bool> Update(TripDto.Request.Update dto);
    public Task<bool> UpdateGpxFile(Guid tripId, Guid gpxFileId);
    public Task<bool> Delete(Guid id);

    public Task<TripDto.Partial?> GetById(Guid id);
    public Task<List<TripDto.Request.ResponseBasic>> GetAll();
}
