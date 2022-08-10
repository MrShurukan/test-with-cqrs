using System.ComponentModel.DataAnnotations.Schema;

namespace SharedClasses;

public abstract class BaseEntity
{
    public int Id { get; set; }

    private readonly List<IBaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<IBaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddEvent(IBaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveEvent(IBaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }
}