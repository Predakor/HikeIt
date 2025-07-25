using Domain.Interfaces;
using Domain.Users.ValueObjects;

namespace Domain.Trips.Events;

public sealed record TripCreatedEvent(Trip Trip, StatsUpdates.All Summary) : IDomainEvent;

