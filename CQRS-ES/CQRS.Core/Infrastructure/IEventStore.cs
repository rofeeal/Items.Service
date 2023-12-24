using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure
{
    public interface IEventStore<T>
    {
        Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion, string aggregateType);
        Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
        Task<List<Guid>> GetAggregateIdsByTypeAsync(string aggregateType);
        Task<List<EventModel>> GetAggregatesAsync();
    }
}
