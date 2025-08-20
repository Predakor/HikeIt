using Application.Interfaces;
using Domain.Trips.Events;
using Domain.Users;
using Domain.Users.Extentions;

namespace Application.Users.Stats.EventHandlers;

internal sealed class TripRemovedEventHandler : IDomainEventHandler<TripRemovedEvent> {
    readonly IUserRepository _userRepository;

    public TripRemovedEventHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task Handle(
        TripRemovedEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        Console.WriteLine("Trip removed event detected: Updating user stats");

        var trip = domainEvent.Trip;
        if (trip.Analytics is null) {
            Console.WriteLine("Trip removed event detected: no analytics changes to apply");
            return;
        }

        var tripDay = trip.TripDay;
        var statsUpdate = trip.Analytics.ToStatUpdate(tripDay);

        await _userRepository.UpdateStats(trip.UserId, statsUpdate, UpdateMode.Decrease);
    }
}
