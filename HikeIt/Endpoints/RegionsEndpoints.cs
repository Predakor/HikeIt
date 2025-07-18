using Api.Extentions;
using Application.Services.Regions;
using Domain.Common.Result;
using Domain.Entiites.Regions;

namespace Api.Endpoints;

public static class RegionsEndpoints {
    public static RouteGroupBuilder MapRegionsEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/regions");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapGet("/{id}/peaks", GetAllFromRegion);

        return group;
    }

    static async Task<IResult> GetAllFromRegion(
        IRegionRepository repository,
        IRegionService service,
        int id
    ) {
        return await repository
            .GetByIdAsync(id)
            .BindAsync(service.GetAllFromRegion)
            .ToApiResultAsync();
    }

    static async Task<IResult> GetAll(IRegionService service) {
        return await service.GetAllAsync().ToApiResultAsync();
    }

    static async Task<IResult> GetById(IRegionService service, int id) {
        return await service.GetAsync(id).ToApiResultAsync();
    }
}
