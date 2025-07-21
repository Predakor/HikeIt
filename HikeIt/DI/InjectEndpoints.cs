using Api.Endpoints;

namespace Api.DI;

internal static partial class DIextentions {
    public static void MapEndpoints(this WebApplication app) {
        app.MapPeaksEndpoints();
        app.MapRegionsEndpoints();
        app.MapFilesEndpoints().RequireAuthorization();
    }
}
