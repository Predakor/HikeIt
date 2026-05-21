using Application.Commons.Abstractions.Queries;
using Application.Users.RegionProgressions.Dtos;
using Domain.Trips.Analytics.Root;

namespace Application.Users.Regions;

public interface IUserRegionsQueries : IQueryService
{
    Task<Result<RegionTrip[]>> GetTrips(Guid userId, int regionId, CancellationToken ct);
    Task<Result<RoutePath[]>> GetRouteVisualizations(Guid userId, int regionId, CancellationToken ct);
    Task<Result<RegionData>> GetRegionProgess(Guid userId, int RegionId, CancellationToken ct);

    sealed record RegionTrip(Guid Id, string Name, DateOnly TripDay, int? Distance, TimeSpan? Duration);
    sealed record RegionData(RegionProgressDto.Full Progress, bool HasTrips, bool HasVisualizations);
}
