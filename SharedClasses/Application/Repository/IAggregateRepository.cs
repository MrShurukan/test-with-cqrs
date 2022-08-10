using SharedClasses.Application.Aggregate;

namespace SharedClasses.Application.Repository;

/*public interface IAggregateRepository<out T> where T : IAggregate
{
    IEnumerable<T> GetMultiple();
    T? GetFirstOrDefault() => GetMultiple().FirstOrDefault();
    
    void Save();
}

public interface IAsyncAggregateRepository<out T> : IAggregateRepository<T> where T : IAggregate
{
    IAsyncEnumerable<T> GetMultipleAsync();
    T? GetFirstOrDefault() =>GetMultiple();
    
    void Save();
}

public interface I*/