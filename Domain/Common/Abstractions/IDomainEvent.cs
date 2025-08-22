namespace Domain.Common.Abstractions;
public interface IEvent;
public interface IDomainEvent : IEvent;
public interface IBackgroundEvent : IDomainEvent;