namespace SharedClasses.Application.Aggregate;

/// <summary>
/// Описывает агрегат типа <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">Дочерний тип, которым управляет агрегат</typeparam>
public abstract class Aggregate<T> : IAggregate
{
    protected T Entity;

    protected Aggregate(T entity)
    {
        Entity = entity;
    }
}

public interface IAggregate {}