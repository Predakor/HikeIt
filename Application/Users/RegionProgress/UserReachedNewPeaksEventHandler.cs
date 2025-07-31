using Application.Interfaces;
using Application.Mountains;
using Domain.Common.Result;
using Domain.ReachedPeaks.ValueObjects;
using Domain.TripAnalytics.Events;
using Domain.Users;
using Domain.Users.RegionProgres.Factories;
using Domain.Users.RegionProgresses.ValueObjects;
using System.Collections.Immutable;

namespace Application.Users.RegionProgress;

internal class UserReachedNewPeaksEventHandler : IDomainEventHandler<UserReachedNewPeaksEvent> {
    readonly IUserRepository _userRepository;
    readonly IRegionQueryService _regionQueries;

    public UserReachedNewPeaksEventHandler(
        IUserRepository userRepository,
        IRegionQueryService regionQueries
    ) {
        _userRepository = userRepository;
        _regionQueries = regionQueries;
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

    internal async Task<bool> UpdateRegionProgresses(User user, ReachedPeakData[] NewPeaks) {
        var regionUpdates = NewPeaks
            .GroupBy(p => p.RegionID)
            .Select(g => new UpdateRegionProgress(g.Key, g.Select(p => p.PeakId)))
            .ToImmutableArray();

        foreach (var item in regionUpdates) {
            var regionProgress = user.RegionProgresses.FirstOrDefault(rp =>
                rp.RegionId == item.RegionId
            );

            if (regionProgress is null) {
                await _regionQueries
                    .GetPeakCount(item.RegionId)
                    .MapAsync(peaksInRegionCount =>
                        RegionProgressFactory.FromProgressUpdate(
                            item,
                            user.Id,
                            (short)peaksInRegionCount
                        )
                    )
                    .MapAsync(_userRepository.CreateRegionProgress);

                continue;
            }

            user.UpdateRegionProgress(item);
        }

        return true;
    }
}
