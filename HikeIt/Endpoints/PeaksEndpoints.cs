using Api.Extentions;
using Application.Mountains;

namespace Api.Endpoints;

public static class PeaksEndpoints {
    public static RouteGroupBuilder MapPeaksEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/peaks");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);

        return group;
    }

    static async Task<IResult> GetAll(IPeaksQueryService service) {
        return await service.GetAllAsync().ToApiResultAsync();
    }

    static async Task<IResult> GetById(int id, IPeaksQueryService service) {
        return await service.GetByIdAsync(id).ToApiResultAsync();
    }
}
