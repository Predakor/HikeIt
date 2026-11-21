using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Root;

namespace Application.Trips.Analytics.Pipeline;

// What the preview shows of a stage's output: the client data and the point count (null when the stage has none).
public sealed record StageView(object Data, int? PointCount);

internal static class StagePresenter
{
    private static readonly IReadOnlyDictionary<StageId, Func<Stage.Output, PipelineInput, StageView>> Presenters =
        new Dictionary<StageId, Func<Stage.Output, PipelineInput, StageView>>
        {
            [StageId.Cleanup] = PresentCleanup,
            [StageId.ElevationProfile] = (output, _) => new StageView(output.Data, ((ElevationProfileDto)output.Data).Gains.Length + 1),
            [StageId.PeakMatching] = (output, _) => new StageView(output.Data, ((StageData.PeakMatching)output.Data).Matched.Count),
            [StageId.Route] = (output, _) => new StageView(output.Data, ((RoutePath)output.Data).Points.Count),
            [StageId.Terrain] = (output, _) => new StageView(output.Data, null),
            [StageId.Pace] = (output, _) => new StageView(output.Data, null),
        };

    public static StageView Present(StageId stage, Stage.Output output, PipelineInput input)
    {
        return Presenters[stage](output, input);
    }

    // The client gets the counts only; the cleaned points stay in the pipe for the next stages.
    private static StageView PresentCleanup(Stage.Output output, PipelineInput input)
    {
        var cleanedCount = ((AnalyticData)output.Data).Points.Count;

        return new StageView(new StageData.Cleanup(input.RawPoints.Count, cleanedCount), cleanedCount);
    }
}
