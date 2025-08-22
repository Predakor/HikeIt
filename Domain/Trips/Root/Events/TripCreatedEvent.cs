using Domain.Common.Abstractions;
using Domain.Users.Stats.ValueObjects;

namespace Domain.Trips.Root.Events;

public sealed record TripCreatedEvent(Trip Trip, UserStatsUpdates.All Summary) : IDomainEvent;

