using Domain.Common;
using Domain.Interfaces;
using Domain.ReachedPeaks.ValueObjects;

namespace Domain.TripAnalytics.Events;

public record UserReachedNewPeaksEvent(ResourceOperation<Guid, ReachedPeakData[]> Data) : IDomainEvent;
