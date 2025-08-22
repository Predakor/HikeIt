using Domain.Common.Abstractions;
using Domain.Users.Stats.ValueObjects;

namespace Domain.Trips.Root.Events;

public sealed record TripAnalyticsCreatedEvent(Guid TripId, Guid OwnerId, UserStatsUpdates.All StatUpdate) : IDomainEvent;

