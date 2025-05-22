using Application.Services.Peaks;

namespace Api.Endpoints;

public static class PeaksEndpoints {

    public static RouteGroupBuilder MapPeaksEndpoint(this WebApplication app) {
        var group = app.MapGroup("api/peaks");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);

        return group;
    }

    static async Task<IResult> GetAll(PeakService service) {
        var peaks = await service.GetAllPeaksAsync();
        var peakList = peaks.ToList();

        if (peakList.Count == 0) {
            return Results.NotFound();
        }

        return Results.Ok(peakList);
    }

    static async Task<IResult> GetById(int id, PeakService service) {
        var peakDto = await service.GetPeakByIdAsync(id);
        if (peakDto == null) {
            return Results.NotFound();
        }

        return Results.Ok(peakDto);
    }

}
