using Application.Commons.Interfaces;
using Application.Dto;
using Domain.Common;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Entities.PeaksAnalytics;

namespace Application.TripAnalytics.Commands;

record CommandData(List<PeakDto.Reached> ReachedPeaks, Guid TripId, Guid UserId);

internal class CreatePeakAnalyticsCommand(CommandData data) : ICommand<PeaksAnalytic> {
    readonly List<PeakDto.Reached> _reachedPeaks = data.ReachedPeaks;
    readonly Guid _tripId = data.TripId;
    readonly Guid _userId = data.UserId;

    public Result<PeaksAnalytic> Execute() {
        if (_reachedPeaks.Count == 0) {
            var error = Error.Unknown("Passed empty array");
            return CommandResult.Failure(error);
        }

        List<ReachedPeak> peaks = _reachedPeaks
            .Select(p => ReachedPeak.Create(_tripId, _userId, p.Id))
            .ToList();

        var peakAnalytics = PeaksAnalytic.Create(peaks);
        if (peakAnalytics == null) {
            var error = Error.Unknown("There was error while generating your peak analytics");
            return CommandResult.Failure(error);
        }

        return CommandResult.Success(peakAnalytics);
    }

    public static ICommand<PeaksAnalytic> Create(CommandData data) {
        return new CreatePeakAnalyticsCommand(data);
    }

    static class CommandResult {
        public static Result<PeaksAnalytic> Success(PeaksAnalytic analytics) {
            return Result<PeaksAnalytic>.Success(analytics);
        }

        public static Result<PeaksAnalytic> Failure(Error error) {
            return Result<PeaksAnalytic>.Failure(error);
        }
    }
}
