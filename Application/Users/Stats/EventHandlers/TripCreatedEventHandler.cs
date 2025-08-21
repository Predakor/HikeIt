using Application.Commons.Interfaces;
using Domain.Trips.Events;
using Domain.Users;

namespace Application.Users.Stats.EventHandlers;

internal sealed class TripCreatedEventHandler : IDomainEventHandler<TripAnalyticsCreatedEvent> {
    readonly IUserRepository _userRepository;

    public TripCreatedEventHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task Handle(
        TripAnalyticsCreatedEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        Console.WriteLine("Trip added event detected: Updating user stats");

        await _userRepository.UpdateStats(domainEvent.Trip.UserId, domainEvent.StatUpdate);
    }
}
