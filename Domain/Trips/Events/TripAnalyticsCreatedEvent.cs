using Domain.Interfaces;
using Domain.Users.ValueObjects;

namespace Domain.Trips.Events;

public sealed record TripAnalyticsCreatedEvent(Guid TripId, Guid OwnerId, StatsUpdates.All StatUpdate) : IDomainEvent;

