using Application.Locations.Peaks;
using Domain.Common.Abstractions;
using Domain.ReachedPeaks;

namespace Application.Trips.Analytics.PeakAnalytics.Commands;

internal record CommandData(List<PeakDto.Reached> ReachedPeaks, Guid TripId, Guid UserId);

internal class CreateReachedPeaksCommand(CommandData data) : ICommand<List<ReachedPeak>>
{
    private readonly List<PeakDto.Reached> _reachedPeaks = data.ReachedPeaks;
    private readonly Guid _tripId = data.TripId;
    private readonly Guid _userId = data.UserId;

    public static ICommand<List<ReachedPeak>> Create(CommandData data)
    {
        return new CreateReachedPeaksCommand(data);
    }

    public Result<List<ReachedPeak>> Execute()
    {
        if (_reachedPeaks.Count == 0)
        {
            return Errors.Unknown("Passed empty array");
        }

        List<ReachedPeak> peaks = _reachedPeaks
            .Select(p => ReachedPeak.Create(p.Id, _tripId, _userId))
            .ToList();

        return peaks;
    }
}
