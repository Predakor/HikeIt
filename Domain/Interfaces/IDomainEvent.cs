namespace Domain.Interfaces;
public interface IEvent;
public interface IDomainEvent : IEvent;
public interface IBackgroundEvent : IDomainEvent;