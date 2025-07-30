using Domain.Common;
using Domain.Interfaces;
using Domain.ReachedPeaks;

namespace Domain.TripAnalytics.Events;

public record UserReachedNewPeaksEvent(ResourceOperation<Guid, ReachedPeak[]> Data) : IDomainEvent;
