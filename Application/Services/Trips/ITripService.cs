using Domain.Common.Result;
using Domain.Entiites.Users;
using Domain.Trips;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Http;
using static Application.Dto.TripDto;

namespace Application.Services.Trips;

public interface ITripService {
    public Task<Result<Guid>> Add(Request.Create dto, Guid userId);
    public Task<Result<Guid>> Add(Request.Create dto, AnalyticData data, User user, Guid tripId);

    public Task<Result<Trip>> Create(Request.Create, IFormFile file, User user);

    public Task<bool> Delete(Guid id, Guid userId);

    public Task<Partial2?> GetById(Guid id, Guid userId);
    public Task<Result<List<Request.ResponseBasic>>> GetAll(Guid userId);
}
