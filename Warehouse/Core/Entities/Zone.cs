using System.Diagnostics.Contracts;
using Core.Exceptions;
using SharedClasses;
using SharedClasses.Application.Aggregate;

namespace Core.Entities;

#region Агрегат

public class ZoneAggregate : Aggregate<Zone>
{
    public ZoneAggregate(Zone zone) : base(zone)
    {
    }

    public void AddContainer(Container container)
    {
        var costForStoringContainer = Entity.CalculateCostForStoring(container);
        if (Entity.CurrentCapacity + costForStoringContainer > Entity.MaxCapacity)
        {
            ExtendZoneCapacity(costForStoringContainer);
        }
        
        Entity.Containers.Add(container);
        Entity.AddEvent(new ContainerAddedToZoneEvent(Entity, container));
    }

    public void RemoveContainer(Container container)
    {
        Entity.Containers.Remove(container);
        Entity.AddEvent(new ContainerRemovedFromZoneEvent(Entity, container));
    }

    private const double MaximumPossibleCapacity = 1000.0;
    public void ExtendZoneCapacity(double amount)
    {
        Entity.MaxCapacity += amount;
        
        // Случайное правило, чтобы жизнь не казалась мёдом
        if (Entity.MaxCapacity >= MaximumPossibleCapacity)
            throw new ZoneExceededMaximumCapacityException(MaximumPossibleCapacity, Entity.MaxCapacity);
        
        Entity.AddEvent(new ZoneCapacityExtended(Entity, Entity.MaxCapacity, amount));
    }
}

#endregion

#region Сущность

public class Zone : BaseEntity
{
    public List<Container> Containers { get; private set; }
    public double CostToStoreForSmallContainer => CalculateCostForStoringSingleContainer(MaxCapacity);
    public double MaxCapacity { get; set; }
    public double CurrentCapacity => Containers.Sum(CalculateCostForStoring);

    [Pure]
    public double CalculateCostForStoring(Container container)
    {
        return container.ContainerType switch
        {
            ContainerType.Small => CostToStoreForSmallContainer,
            ContainerType.Medium => CostToStoreForSmallContainer * 2,
            ContainerType.Large => CostToStoreForSmallContainer * 3,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [Pure]
    private double CalculateCostForStoringSingleContainer(double maxCapacity) => maxCapacity * 2 / 3;
}

#endregion

#region События

public class ZoneCapacityExtended : IDomainEvent
{
    public Zone Zone { get; }
    public double NewCapacity { get; }
    public double ExtendedBy { get; }
    
    public ZoneCapacityExtended(Zone zone, double newCapacity, double extendedBy)
    {
        Zone = zone;
        NewCapacity = newCapacity;
        ExtendedBy = extendedBy;
    }
}

public class ContainerAddedToZoneEvent : IDomainEvent
{
    public Zone Zone { get; }
    public Container Container { get; }

    public ContainerAddedToZoneEvent(Zone zone, Container container)
    {
        Zone = zone;
        Container = container;
    }
}

public class ContainerRemovedFromZoneEvent : IDomainEvent
{
    public Zone Zone { get; }
    public Container Container { get; }

    public ContainerRemovedFromZoneEvent(Zone zone, Container container)
    {
        Zone = zone;
        Container = container;
    }
}

#endregion