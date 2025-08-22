using Application.Commons.Abstractions;
using Domain.Common.Result;
using Domain.Trips.Root.Events;
using Domain.Users.Root;

namespace Application.Users.Stats.EventHandlers;

internal sealed class TripDateUpdated_UpdateStatsHandler : IDomainEventHandler<TripDateUpdatedEvent> {
    readonly IUserRepository _userRepository;

    public TripDateUpdated_UpdateStatsHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task Handle(
        TripDateUpdatedEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        await _userRepository
            .GetUserStats(domainEvent.Trip.UserId)
            .TapAsync(stats => stats.UpdateFirstLastTripDate(domainEvent.Trip.TripDay))
            .TapAsync(_ => _userRepository.SaveChangesAsync());
    }
}
