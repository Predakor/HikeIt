using Application.Trips;
using Domain.Common.Result;
using Domain.Trips;
using static Application.Dto.TripDto;

namespace Application.Services.Trips;

public interface ITripService {
    public Task<Result<Partial2>> GetByIdAsync(Guid id, Guid userId);
    public Task<Result<List<Request.ResponseBasic>>> GetAllAsync(Guid userId);

    public Task<Result<Trip>> CreateAsync(CreateTripContext ctx);
    public Task<Result<Guid>> CreateSimpleAsync(CreateTripContext ctx);

    public Task<Result<bool>> DeleteAsync(Guid id, Guid userId);
}
