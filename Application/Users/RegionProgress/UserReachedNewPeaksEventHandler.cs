using Application.Interfaces;
using Domain.Common.Result;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Events;
using Domain.Users;
using Domain.Users.RegionProgresses.ValueObjects;
using System.Collections.Immutable;

namespace Application.Users.RegionProgress;

record NewPeakEventData(int PeakId, int RegionId);

internal class UserReachedNewPeaksEventHandler : IDomainEventHandler<UserReachedNewPeaksEvent> {
    readonly IUserRepository _userRepository;

    public UserReachedNewPeaksEventHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task Handle(
        UserReachedNewPeaksEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        var (UserId, _, NewPeaks) = domainEvent.Data;

        await _userRepository
            .GetWithRegionProgresses(UserId)
            .MapAsync(user => UpdateRegionProgresses(user, NewPeaks))
            .BindAsync(_ => _userRepository.SaveChangesAsync());
    }

    internal static bool UpdateRegionProgresses(User user, ReachedPeak[] NewPeaks) {
        var regionUpdates = ToRegionUpdates(NewPeaks);
        foreach (var item in regionUpdates) {
            user.UpdateOrAddRegionProgress(item);
        }
        return true;
    }

    internal static ImmutableArray<UpdateRegionProgress> ToRegionUpdates(ReachedPeak[] NewPeaks) {
        return NewPeaks
            .Select(rp => new NewPeakEventData(rp.PeakId, rp.Peak.RegionID))
            .GroupBy(p => p.RegionId)
            .Select(g => new UpdateRegionProgress(g.Key, g.Select(p => p.PeakId)))
            .ToImmutableArray();
    }
}
