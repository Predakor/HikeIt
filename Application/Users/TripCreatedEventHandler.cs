using Application.Interfaces;
using Domain.Trips;
using Domain.Users;

namespace Application.Users;

internal class TripCreatedEventHandler : IDomainEventHandler<TripAnalyticsAddedDomainEvent> {
    readonly IUserRepository _userRepository;

    public TripCreatedEventHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task Handle(
        TripAnalyticsAddedDomainEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        await _userRepository.UpdateStats(domainEvent.UserId, domainEvent.Summary);
        return;
    }
}
