using Application.Commons.Interfaces;
using Domain.Common.Result;
using Domain.Trips.Events;
using Domain.Users;

namespace Application.Users.Stats.EventHandlers;

internal sealed class TripDateUpdatedEventHandler : IDomainEventHandler<TripDateUpdatedEvent> {
    readonly IUserRepository _userRepository;

    public TripDateUpdatedEventHandler(IUserRepository userRepository) {
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
