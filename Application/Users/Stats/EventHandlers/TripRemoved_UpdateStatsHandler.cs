using Application.Commons.Abstractions;
using Domain.Trips.Analytics.Root.Extentions;
using Domain.Trips.Root.Events;
using Domain.Users.Root;
using Domain.Users.Stats.Extentions;

namespace Application.Users.Stats.EventHandlers;

internal sealed class TripRemoved_UpdateStatsHandler : IDomainEventHandler<TripRemovedEvent> {
    readonly IUserRepository _userRepository;

    public TripRemoved_UpdateStatsHandler(IUserRepository userRepository) {
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

        var statsUpdate = trip.Analytics.ToStatUpdate();

        await _userRepository.UpdateStats(trip.UserId, statsUpdate, UpdateMode.Decrease);
    }
}
