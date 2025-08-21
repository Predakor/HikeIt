using Application.Commons.Interfaces;
using Application.Mountains;
using Domain.Common.Result;
using Domain.ReachedPeaks.ValueObjects;
using Domain.TripAnalytics.Events;
using Domain.Users;
using Domain.Users.RegionProgres.Factories;
using Domain.Users.RegionProgresses.ValueObjects;
using System.Collections.Immutable;

namespace Application.Users.RegionProgresses;

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
                await CreateNewRegionProgress(user, item);

                continue;
            }

            user.UpdateRegionProgress(item);
        }

        return true;
    }

    async Task CreateNewRegionProgress(User user, UpdateRegionProgress progressUpdate) {
        await _regionQueries
            .GetPeakCount(progressUpdate.RegionId)
            .MapAsync(peaksInRegionCount =>
                RegionProgressFactory.FromProgressUpdate(
                    progressUpdate,
                    user.Id,
                    (short)peaksInRegionCount
                )
            )
            .MapAsync(_userRepository.CreateRegionProgress);
    }
}
