using Domain.Interfaces;

namespace Domain.Trips.Events;

public sealed record TripRemovedEvent(Trip Trip) : IDomainEvent;
