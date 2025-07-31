using Application.Interfaces;
using Domain.Common.Result;
using Domain.Trips.Events;
using Domain.Users;
using Domain.Users.RegionProgresses;

namespace Application.Users.RegionProgresses.EventHandlers;

internal class ReachedPeakRemovedEventHandler : IDomainEventHandler<ReachedPeakRemovedEvent> {
    readonly IUserRepository _userRepository;

    public ReachedPeakRemovedEventHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task Handle(
        ReachedPeakRemovedEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        var regionUpdates = MergeRegionUpdates(domainEvent.RemovedPeaks);

        await _userRepository
            .GetWithRegionProgresses(domainEvent.UserId)
            .MapAsync(user => user.RegionProgresses.ToList())
            .MapAsync(progresses => UpdateUserRegionsSummaries(regionUpdates, progresses))
            .MapAsync(_ => _userRepository.SaveChangesAsync());
    }

    static Dictionary<int, PeakUpdateData[]> MergeRegionUpdates(PeakUpdateData[] removedPeaks) {
        return removedPeaks.GroupBy(ru => ru.RegionId).ToDictionary(g => g.Key, g => g.ToArray());
    }

    internal static Task UpdateUserRegionsSummaries(
        Dictionary<int, PeakUpdateData[]> regionUpdates,
        List<RegionProgress> userRegionsProgresses
    ) {
        foreach (var regionUpdate in regionUpdates) {
            var regionToUpdate = userRegionsProgresses.FirstOrDefault(p =>
                p.RegionId == regionUpdate.Key
            );

            if (regionToUpdate is null) {
                continue;
            }

            regionToUpdate.RemovePeakVisits(regionUpdate.Value.Select(x => x.PeakId));
        }
        return Task.CompletedTask;
    }
}
