using Domain.Common.Abstractions;

namespace Domain.Trips.Root.Events;

public sealed record GpxFileAttatchedEvent(Guid FileId, Guid TripId) : IDomainEvent;
