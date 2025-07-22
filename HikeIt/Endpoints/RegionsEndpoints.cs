using Api.Extentions;
using Application.Mountains;
using Domain.Common.Result;

namespace Api.Endpoints;

public static class RegionsEndpoints {
    public static RouteGroupBuilder MapRegionsEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/regions");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapGet("/{id}/peaks", GetAllFromRegion);

        return group;
    }

    static async Task<IResult> GetAllFromRegion(IRegionQueryService queries, int id) {
        return await queries
            .GetById(id)
            .BindAsync(queries.AllPeaksFromRegion)
            .ToApiResultAsync();
    }

    static async Task<IResult> GetAll(IRegionQueryService service) {
        return await service.GetAllAsync().ToApiResultAsync();
    }

    static async Task<IResult> GetById(IRegionQueryService service, int id) {
        return await service.GetById(id).ToApiResultAsync();
    }
}
