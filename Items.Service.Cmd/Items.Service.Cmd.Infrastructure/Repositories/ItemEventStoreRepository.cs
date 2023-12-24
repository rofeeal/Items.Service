using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Items.Service.Cmd.Infrastructure.Config;
using Items.Service.Cmd.Infrastructure.Interfaces;

namespace Items.Service.Cmd.Infrastructure.Repositories
{
    public class ItemEventStoreRepository : IItemEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public ItemEventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

            _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.ItemCollection);
        }

        public async Task<List<EventModel>> FindAllAsync()
        {
            return await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            return await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindByAggregateType(string aggregateType)
        {
            return await _eventStoreCollection.Find(x => x.AggregateType == aggregateType).ToListAsync().ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }
    }
}
