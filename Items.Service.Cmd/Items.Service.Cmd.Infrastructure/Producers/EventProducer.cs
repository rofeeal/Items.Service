using Confluent.Kafka;
using Confluent.Kafka.Admin;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Items.Service.Cmd.Infrastructure.Producers
{
    public class EventProducer : IEventProducer
    {
        private readonly ProducerConfig _config;

        public EventProducer(IOptions<ProducerConfig> config)
        {
            _config = config.Value;
        }

        public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
        {
            using var producer = new ProducerBuilder<string, string>(_config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

            var eventMessage = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };

            var deliveryResult = await producer.ProduceAsync(topic, eventMessage);

            if (deliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                throw new Exception($"Could not produce {@event.GetType().Name} message to topic - {topic} due to the following reason: {deliveryResult.Message}.");
            }
        }

        public async Task CreateKafkaTopicIfNotExists(string topicName)
        {
            using var adminClient = new AdminClientBuilder(_config).Build();

            try
            {
                // Check if the topic already exists
                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(20));

                if (!metadata.Topics.Exists(t => t.Topic == topicName))
                {
                    // Create the topic if it doesn't exist
                    await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                        new TopicSpecification { Name = topicName, NumPartitions = 1, ReplicationFactor = 1 }
                        // Adjust partitions and replication factor based on your requirements
                    });
                }
            }
            catch (CreateTopicsException e)
            {
                // Handling topic creation exception
                Console.WriteLine($"An error occurred creating the topic: {e.Results[0].Error.Reason}");
            }
        }
    }
}