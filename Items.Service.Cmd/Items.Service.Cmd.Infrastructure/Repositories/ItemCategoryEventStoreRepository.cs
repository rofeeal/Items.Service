using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Items.Service.Cmd.Infrastructure.Config;
using Items.Service.Cmd.Infrastructure.Interfaces;

namespace Items.Service.Cmd.Infrastructure.Repositories
{
    public class ItemCategoryEventStoreRepository : IItemCategoryEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public ItemCategoryEventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

            _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.ItemCategoryCollection);
        }

        public async Task<List<EventModel>> FindAllAsync()
        {
            return await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            try
            {
                return await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId)
                    .ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Handle the exception here (you can log, throw a custom exception, or handle it as required)
                // For demonstration purposes, let's log the exception message.
                Console.WriteLine($"An error occurred: {ex.Message}");

                // Return an empty list or throw a custom exception based on your requirement.
                return new List<EventModel>(); // Returning an empty list in case of an exception
            }
        }


        public async Task<List<EventModel>> FindByAggregateType(string aggregateCategory)
        {
            return await _eventStoreCollection.Find(x => x.AggregateType == aggregateCategory).ToListAsync().ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
        }
	}
}
