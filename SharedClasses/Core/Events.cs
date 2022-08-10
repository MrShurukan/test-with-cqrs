using MediatR;

namespace SharedClasses;

public interface IBaseEvent
{
}

public interface IDomainEvent : IBaseEvent, INotification
{
}

public interface IIntegrationEvent : IBaseEvent
{
    public string GetIntegrationEventString();
}

// Например, если нужно будет
public interface ISqlEvent : IBaseEvent {}