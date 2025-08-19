using Domain.Interfaces;

namespace Domain.Trips.Events;
public sealed record TripDateUpdatedEvent(Trip Trip) : IDomainEvent;

