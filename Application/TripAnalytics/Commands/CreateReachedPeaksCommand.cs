using Application.Commons.Interfaces;
using Application.Dto;
using Domain.Common;
using Domain.ReachedPeaks;

namespace Application.TripAnalytics.Commands;

record CommandData(List<PeakDto.Reached> ReachedPeaks, Guid TripId, Guid UserId);

internal class CreateReachedPeaksCommand(CommandData data) : ICommand<List<ReachedPeak>> {
    readonly List<PeakDto.Reached> _reachedPeaks = data.ReachedPeaks;
    readonly Guid _tripId = data.TripId;
    readonly Guid _userId = data.UserId;

    public Result<List<ReachedPeak>> Execute() {
        if (_reachedPeaks.Count == 0) {
            var error = Errors.Unknown("Passed empty array");
            return CommandResult.Failure(error);
        }

        List<ReachedPeak> peaks = _reachedPeaks
            .Select(p => ReachedPeak.Create(_tripId, _userId, p.Id))
            .ToList();

        return CommandResult.Success(peaks);
    }

    public static ICommand<List<ReachedPeak>> Create(CommandData data) {
        return new CreateReachedPeaksCommand(data);
    }

    static class CommandResult {
        public static Result<List<ReachedPeak>> Success(List<ReachedPeak> analytics) {
            return Result<List<ReachedPeak>>.Success(analytics);
        }

        public static Result<List<ReachedPeak>> Failure(Error error) {
            return Result<List<ReachedPeak>>.Failure(error);
        }
    }
}
