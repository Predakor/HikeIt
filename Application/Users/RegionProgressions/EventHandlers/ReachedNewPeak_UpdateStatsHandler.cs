using Application.Commons.Abstractions;
using Application.Locations.Regions;
using Domain.ReachedPeaks.ValueObjects;
using Domain.Trips.Analytics.Peaks.Events;
using Domain.Users.RegionProgressions.Factories;
using Domain.Users.RegionProgressions.ValueObjects;
using Domain.Users.Root;
using System.Collections.Immutable;

namespace Application.Users.RegionProgressions.EventHandlers;

internal class ReachedNewPeak_UpdateStatsHandler : IDomainEventHandler<ReachedNewPeak> {
    readonly IUserRepository _userRepository;
    readonly IRegionQueryService _regionQueries;

    public ReachedNewPeak_UpdateStatsHandler(
        IUserRepository userRepository,
        IRegionQueryService regionQueries
    ) {
        _userRepository = userRepository;
        _regionQueries = regionQueries;
    }

    public async Task Handle(
        ReachedNewPeak domainEvent,
        CancellationToken cancellationToken = default
    ) {
        var (UserId, _, NewPeaks) = domainEvent.Data;

        await _userRepository
            .GetWithRegionProgresses(UserId)
            .MapAsync(user => UpdateRegionProgresses(user, NewPeaks))
            .BindAsync(_ => _userRepository.SaveChangesAsync());
    }

    internal async Task<bool> UpdateRegionProgresses(User user, CreateReachedPeak[] NewPeaks) {
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
