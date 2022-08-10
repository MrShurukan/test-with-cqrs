using SharedClasses;
using SharedClasses.Application.Aggregate;

namespace Core.Entities;

#region Репозиторий

public interface IContainerRepository
{
    
}

#endregion

#region Агрегат

public class ContainerAggregate : Aggregate<Container>
{
    public ContainerAggregate(Container container) : base(container)
    {
    }
}

#endregion

#region Сущность

public class Container : BaseAuditableEntity
{
    public ContainerType ContainerType { get; set; }
    
    private Zone _zone;
    public Zone Zone
    {
        get => _zone;
        set
        {
            AddEvent(new ContainerChangedZones(this, value));
            _zone = value;
        }
    }
}

#endregion

#region События

public class ContainerChangedZones : IDomainEvent
{
    public Container Container;
    public Zone Zone;

    public ContainerChangedZones(Container container, Zone zone)
    {
        Container = container;
        Zone = zone;
    }
}

#endregion