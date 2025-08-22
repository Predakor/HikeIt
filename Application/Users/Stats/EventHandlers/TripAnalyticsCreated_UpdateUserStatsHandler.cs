using Application.Commons.Interfaces;
using Domain.Trips.Events;
using Domain.Users;

namespace Application.Users.Stats.EventHandlers;

internal sealed class TripAnalyticsCreated_UpdateUserStatsHandler
    : IDomainEventHandler<TripAnalyticsCreatedEvent> {
    readonly IUserRepository _userRepository;

    public TripAnalyticsCreated_UpdateUserStatsHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task Handle(
        TripAnalyticsCreatedEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        await _userRepository.UpdateStats(domainEvent.OwnerId, domainEvent.StatUpdate);
    }
}
