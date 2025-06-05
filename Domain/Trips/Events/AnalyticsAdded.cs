using Domain.Trips.ValueObjects.TripAnalytics;

namespace Domain.Trips.Events;
public record AnalyticsAdded(TripAnalytic Analytics, Trip Trip) : IDomainEvent;

