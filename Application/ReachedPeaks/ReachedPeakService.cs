using Application.ReachedPeaks.ValueObjects;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;
using Domain.ReachedPeaks;
using Domain.ReachedPeaks.Builders;
using Domain.ReachedPeaks.ValueObjects;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Root;
using Domain.Users.Root;

namespace Application.ReachedPeaks;

public class ReachedPeakService : IReachedPeakService
{
    private readonly IPeaksFilterRunner _peaksRunner;
    private readonly IReachedPeaksQureryService _reachedPeaksQueries;

    public ReachedPeakService(IPeaksFilterRunner peaksRunner, IReachedPeaksQureryService reachedPeaksQueries)
    {
        _peaksRunner = peaksRunner;
        _reachedPeaksQueries = reachedPeaksQueries;
    }

    private static Result<List<ReachedPeakDataBuilder>> ToMatchedBuilders(PeakChain chain)
    {
        return chain
            .Matched.Select(candidate =>
                ReachedPeakDataFactory
                    .FromPointWithDistance(candidate.Point)
                    .WithPeak(candidate.Peak!)
            )
            .EnsureNotEmpty("No peak matched the trip's candidate peaks")
            .Map(builders => builders.ToList());
    }

    private static Result<List<ReachedPeak>> ToReachedPeaks(IEnumerable<ReachedPeakDataBuilder> peaks, Trip trip)
    {
        if (peaks.NullOrEmpty())
        {
            return Errors.EmptyCollection("Peaks");
        }

        Console.WriteLine($"Matched {peaks.Count()} Peaks");

        CreateReachedPeak[] newPeaksEventData = [.. peaks.Select(p => p.Build())];
        trip.AddReachedPeaks(newPeaksEventData);

        return peaks
            .Select(p => ReachedPeak.Create(p.PeakId, trip.Id, trip.UserId, p.TimeReached, p.FirstTime))
            .ToList();
    }

    public Result<ReachedPeak> ToReachedPeak(Peak peak, Trip trip, User user)
    {
        return ReachedPeak.Create(peak, trip, user);
    }

    public async Task<Result<List<ReachedPeak>>> CreateReachedPeaks(AnalyticData data, Trip trip)
    {
        return await _peaksRunner
            .ApplyAsync(PeakCandidate.From(data))
            .BindAsync(ToMatchedBuilders)
            .BindAsync(peaks => MarkNewPeaks(trip.UserId, peaks))
            .BindAsync(foundPeaks => ToReachedPeaks(foundPeaks, trip));
    }

    private async Task<Result<List<ReachedPeakDataBuilder>>> MarkNewPeaks(
        Guid userId,
        List<ReachedPeakDataBuilder> peaks
    )
    {
        var newPeaks = await _reachedPeaksQueries.ReachedByUserBefore(userId, peaks.Select(p => p.PeakId));

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
}
