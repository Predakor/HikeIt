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
        await _userRepository
            .GetWithRegionProgresses(domainEvent.UserId)
            .MapAsync(ProgressionsContext.FromUser)
            .MapAsync(ctx => ctx.AddRegionUpdates(domainEvent.RemovedPeaks))
            .MapAsync(UpdateUserRegionsSummaries)
            .MapAsync(RemoveEmptyRegionProgressions)
            .MapAsync(_ => _userRepository.SaveChangesAsync());
    }

    internal static ProgressionsContext UpdateUserRegionsSummaries(ProgressionsContext ctx) {
        foreach (var regionUpdate in ctx.RegionUpdates) {
            var regionToUpdate = ctx.RegionProgressions.FirstOrDefault(p =>
                p.RegionId == regionUpdate.Key
            );

            if (regionToUpdate is null) {
                continue;
            }

            regionToUpdate.RemovePeakVisits(regionUpdate.Value.Select(x => x.PeakId));
        }
        return ctx;
    }

    internal static ProgressionsContext RemoveEmptyRegionProgressions(ProgressionsContext ctx) {
        var emptyProgressions = ctx
            .User.RegionProgresses.Where(rp => rp.TotalReachedPeaks == 0)
            .ToList();

        foreach (var emptyProgress in emptyProgressions) {
            ctx.User.RegionProgresses.Remove(emptyProgress);
        }

        return ctx;
    }
}

class ProgressionsContext {
    public User User { get; init; }
    public List<RegionProgress> RegionProgressions;
    public Dictionary<int, PeakUpdateData[]> RegionUpdates { get; private set; } = [];

    ProgressionsContext(User user) {
        User = user;
        RegionProgressions = user.RegionProgresses.ToList();
    }

    public ProgressionsContext AddRegionUpdates(PeakUpdateData[] removedPeaks) {
        RegionUpdates = MergeRegionUpdates(removedPeaks);
        return this;
    }

    public static ProgressionsContext FromUser(User user) {
        return new ProgressionsContext(user);
    }

    static Dictionary<int, PeakUpdateData[]> MergeRegionUpdates(PeakUpdateData[] removedPeaks) {
        return removedPeaks.GroupBy(ru => ru.RegionId).ToDictionary(g => g.Key, g => g.ToArray());
    }
}
