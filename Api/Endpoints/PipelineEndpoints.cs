using Api.Commons.Extentions;
using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;

namespace Api.Endpoints;

public static class PipelineEndpoints
{
    public static RouteGroupBuilder MapPipelineEndpoints(this WebApplication app)
    {
        var group = app
            .MapGroup("api/trips/{id}/analytics/pipeline")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        group
            .MapGet("/defaults", GetDefaults)
            .Produces<PipelineConfig>();
        group
            .MapPost("/preview", Preview)
            .Produces<PipelinePreviewDto>();

        return group;
    }

    // What every stage resolves for a null config (DB settings, else code constants); the bench customises from it.
    private static Task<IResult> GetDefaults(IPipelineSettingsService settings, CancellationToken cancellationToken)
    {
        return settings
            .GetDefaultsAsync(cancellationToken)
            .ToApiResultAsync();
    }

    private static Task<IResult> Preview(
        Guid id,
        PipelinePreviewRequest request,
        IPipelinePreviewService preview,
        CancellationToken cancellationToken
    )
    {
        return preview
            .PreviewAsync(id, request, cancellationToken)
            .ToApiResultAsync();
    }
}
