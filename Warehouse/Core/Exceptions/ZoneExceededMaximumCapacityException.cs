namespace Core.Exceptions;

public class ZoneExceededMaximumCapacityException : Exception
{
    private readonly double _zoneCapacity;
    private readonly double _maximumPossibleCapacity;

    public ZoneExceededMaximumCapacityException(double maximumPossibleCapacity, double zoneCapacity)
    {
        _maximumPossibleCapacity = maximumPossibleCapacity;
        _zoneCapacity = zoneCapacity;
    }

    public override string ToString()
    {
        return $"Зона вышла за максимальный размер (она: {_zoneCapacity}, максимум: {_maximumPossibleCapacity})";
    }
}