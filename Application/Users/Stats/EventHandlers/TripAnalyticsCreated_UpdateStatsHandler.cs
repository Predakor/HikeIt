using Application.Commons.Abstractions;
using Domain.Trips.Root.Events;
using Domain.Users.Root;

namespace Application.Users.Stats.EventHandlers;

internal sealed class TripAnalyticsCreated_UpdateStatsHandler
    : IDomainEventHandler<TripAnalyticsCreatedEvent> {
    readonly IUserRepository _userRepository;

    public TripAnalyticsCreated_UpdateStatsHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task Handle(
        TripAnalyticsCreatedEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        await _userRepository.UpdateStats(domainEvent.OwnerId, domainEvent.StatUpdate);
    }
}
