using Application.Services.Region;

namespace Api.Endpoints;

public static class RegionsEndpoints {
    public static RouteGroupBuilder MapRegionsEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/regions");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);

        return group;
    }

    static async Task<IResult> GetAll(RegionService service) {
        var results = await service.GetAllAsync();
        if (results is null) {
            return Results.NotFound();
        }
        return Results.Ok(results);
    }

    static async Task<IResult> GetById(RegionService service, int id) {
        var result = await service.GetAsync(id);
        if (result is null) {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }
}
