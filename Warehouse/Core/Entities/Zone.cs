using System.Diagnostics.Contracts;
using Core.Exceptions;
using SharedClasses;

namespace Core.Entities;

#region Агрегат

public class ZoneAggregate
{
    private readonly Zone _zone;

    public ZoneAggregate(Zone zone)
    {
        _zone = zone;
    }

    public void AddContainer(Container container)
    {
        var costForStoringContainer = _zone.CalculateCostForStoring(container);
        if (_zone.CurrentCapacity + costForStoringContainer > _zone.MaxCapacity)
        {
            ExtendZoneCapacity(costForStoringContainer);
        }
        
        _zone.Containers.Add(container);
        _zone.AddEvent(new ContainerAddedToZoneEvent(_zone, container));
    }

    public void RemoveContainer(Container container)
    {
        _zone.Containers.Remove(container);
        _zone.AddEvent(new ContainerRemovedFromZoneEvent(_zone, container));
    }

    private const double MaximumPossibleCapacity = 1000.0;
    public void ExtendZoneCapacity(double amount)
    {
        _zone.MaxCapacity += amount;
        
        // Случайное правило, чтобы жизнь не казалась мёдом
        if (_zone.MaxCapacity >= MaximumPossibleCapacity)
            throw new ZoneExceededMaximumCapacityException(MaximumPossibleCapacity, _zone.MaxCapacity);
        
        _zone.AddEvent(new ZoneCapacityExtended(_zone, _zone.MaxCapacity, amount));
    }
}

#endregion

#region Сущность

public class Zone : BaseEntity
{
    public string Name { get; set; }
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