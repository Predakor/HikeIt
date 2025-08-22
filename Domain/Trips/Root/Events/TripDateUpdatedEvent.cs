using Domain.Common.Abstractions;

namespace Domain.Trips.Root.Events;
public sealed record TripDateUpdatedEvent(Trip Trip) : IDomainEvent;

