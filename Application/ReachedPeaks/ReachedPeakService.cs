using Application.Locations.Peaks;
using Application.ReachedPeaks.ValueObjects;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;
using Domain.ReachedPeaks;
using Domain.ReachedPeaks.Builders;
using Domain.ReachedPeaks.ValueObjects;
using Domain.Trips.Root;
using Domain.Users.Root;

namespace Application.ReachedPeaks;

public class ReachedPeakService : IReachedPeakService
{
    private readonly IPeaksQueryService _peaksQueries;
    private readonly IReachedPeaksQureryService _reachedPeaksQueries;
    private readonly IPipelineSettingsService _pipelineSettings;

    public ReachedPeakService(
        IPeaksQueryService peaksQueryService,
        IReachedPeaksQureryService reachedPeaksQueries,
        IPipelineSettingsService pipelineSettings
    )
    {
        _peaksQueries = peaksQueryService;
        _reachedPeaksQueries = reachedPeaksQueries;
        _pipelineSettings = pipelineSettings;
    }

    public Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user)
    {
        return ReachedPeak.Create(peak, trip, user);
    }

    public async Task<Result<List<ReachedPeak>>> CreateReachedPeaks(AnalyticData data, Trip trip)
    {
        var filters = await _pipelineSettings.GetFiltersAsync<IPeakFilterConfig>(StageId.PeakMatching, CancellationToken.None);

        return await new PeakFilterChain(_peaksQueries)
            .ApplyAsync(PeakFilterChain.CandidatesFrom(data), filters)
            .BindAsync(ToMatchedBuilders)
            .BindAsync(peaks => MarkNewPeaks(trip.UserId, peaks))
            .BindAsync(foundPeaks => ToReachedPeaks(foundPeaks, trip));
    }

    private static Result<List<ReachedPeakDataBuilder>> ToMatchedBuilders(PeakChainResult chain)
    {
        return chain
            .ToMatched()
            .Select(candidate => ReachedPeakDataFactory
                .FromPointWithDistance(candidate.Point)
                .WithPeak(candidate.Peak!))
            .EnsureNotEmpty("No peak matched the trip's candidate peaks")
            .Map(builders => builders.ToList());
    }

    private async Task<Result<List<ReachedPeakDataBuilder>>> MarkNewPeaks(
        Guid userId,
        List<ReachedPeakDataBuilder> peaks
    )
    {
        var newPeaks = await _reachedPeaksQueries.ReachedByUserBefore(
            userId,
            peaks.Select(p => p.PeakId)
        );

        if (newPeaks.NullOrEmpty())
        {
            return peaks;
        }

        foreach (var newPeak in newPeaks)
        {
            var firstMathchingPeak = peaks.FirstOrDefault(p => p.PeakId == newPeak.Id);
            if (firstMathchingPeak is not null)
            {
                firstMathchingPeak.SetFirstTimeReached(true);
            }
        }

        return peaks;
    }

    private static Result<List<ReachedPeak>> ToReachedPeaks(
        IEnumerable<ReachedPeakDataBuilder> peaks,
        Trip trip
    )
    {
        if (peaks.NullOrEmpty())
        {
            return Errors.EmptyCollection("Peaks");
        }

        Console.WriteLine($"Matched {peaks.Count()} Peaks");

        CreateReachedPeak[] newPeaksEventData = [.. peaks.Select(p => p.Build())];
        trip.AddReachedPeaks(newPeaksEventData);

        return peaks
            .Select(p =>
                ReachedPeak.Create(p.PeakId, trip.Id, trip.UserId, p.TimeReached, p.FirstTime)
            )
            .ToList();
    }
}
