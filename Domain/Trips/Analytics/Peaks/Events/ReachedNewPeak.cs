using Domain.Common.Abstractions;
using Domain.Common.ValueObjects;
using Domain.ReachedPeaks.ValueObjects;

namespace Domain.Trips.Analytics.Peaks.Events;

public sealed record ReachedNewPeak(ResourceOperation<Guid, CreateReachedPeak[]> Data) : IDomainEvent;
