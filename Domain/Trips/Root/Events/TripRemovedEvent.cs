using Domain.Common.Abstractions;
using Domain.Trips.Root;

namespace Domain.Trips.Root.Events;

public sealed record TripRemovedEvent(Trip Trip) : IDomainEvent;
