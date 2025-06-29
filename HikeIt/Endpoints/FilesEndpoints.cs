using Domain.Trips.Entities.GpxFiles;

namespace Api.Endpoints;

public static class FilesEndpoints {
    public static RouteGroupBuilder MapFilesEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/files");

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //TODO actually add user validation
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        group.MapGet("", GetAll);
        group.MapGet("/{id}", GetById);

        group.MapDelete("/{id}", DeleteById);

        return group;
    }

    static async Task<IResult> GetAll(IGpxFileRepository repos) {
        var res = await repos.GetAllAsync();
        return Results.Ok(res);
    }

    static async Task<IResult> GetById(Guid id, IGpxFileRepository repos) {
        var res = await repos.GetGpxFile(id);

        return Results.Ok(res);
    }

    static Task DeleteById(int id) {
        throw new NotImplementedException();
    }
}
