using Domain.Interfaces;

namespace Domain.Trips.Events;

public sealed record GpxFileAttatchedEvent(Guid FileId, Guid TripId) : IDomainEvent;
