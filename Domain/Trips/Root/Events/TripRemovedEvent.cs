using Domain.Common.Abstractions;

namespace Domain.Trips.Root.Events;

public sealed record TripRemovedEvent(Trip Trip) : IDomainEvent;
