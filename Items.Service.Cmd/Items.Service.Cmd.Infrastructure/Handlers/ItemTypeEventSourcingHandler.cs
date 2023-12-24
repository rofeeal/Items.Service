using CQRS.Core.Domain;
using CQRS.Core.Exceptions;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Items.Service.Cmd.Domain.Aggregates;

namespace Items.Service.Cmd.Infrastructure.Handlers
{
    public class ItemTypeEventSourcingHandler : IEventSourcingHandler<ItemTypeAggregate>
    {
        private readonly IEventStore<ItemTypeAggregate> _eventStore;
        private readonly IEventProducer _eventProducer;

        public ItemTypeEventSourcingHandler(IEventStore<ItemTypeAggregate> eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }

        public async Task<ItemTypeAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new ItemTypeAggregate();

            try
            {
                var events = await _eventStore.GetEventsAsync(aggregateId);

                if (events == null || !events.Any()) return aggregate;

                aggregate.ReplayEvents(events);
                aggregate.Version = events.Select(x => x.Version).Max();
            }
            catch (AggregateNotFoundException ex)
            {
                throw ex;
            }

            return aggregate;
        }


        public async Task RepublishEventsAsync(string aggregateType)
        {
            var aggregateIds = await _eventStore.GetAggregateIdsByTypeAsync(aggregateType);

            if (aggregateIds == null || !aggregateIds.Any()) return;

            foreach (var aggregateId in aggregateIds)
            {
                var aggregate = await GetByIdAsync(aggregateId);

                if (aggregate == null) continue;

                var events = await _eventStore.GetEventsAsync(aggregateId);

                foreach (var @event in events)
                {
                    var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                    await _eventProducer.ProduceAsync(topic, @event);
                }
            }
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version, aggregate.GetType().Name);
            aggregate.MarkChangesAsCommitted();
        }
    }
}