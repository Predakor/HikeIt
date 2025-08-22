using Domain.Common.Abstractions;

namespace Domain.Trips.Root.Events;

public record PeakUpdateData(int PeakId, int RegionId);

public sealed record RemovedReachedPeaksEvent(Guid UserId, PeakUpdateData[] RemovedPeaks)
    : IDomainEvent;
