using Domain.Interfaces;
using Domain.Users.ValueObjects;

namespace Domain.Trips.Events;

public sealed record TripAnalyticsCreatedEvent(Trip Trip, StatsUpdates.All StatUpdate) : IDomainEvent;

