using CQRS.Core.Events;
using System.Text.Json.Serialization;
using System.Text.Json;
using Items.Service.Common.Events;

namespace Items.Service.Query.Infrastructure.Converters
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(Type type)
        {
            return type.IsAssignableFrom(typeof(BaseEvent));
        }

        public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
            {
                throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
            }

            if (!doc.RootElement.TryGetProperty("Type", out var type))
            {
                throw new JsonException("Could not detect the Type discriminator property!");
            }

            var typeDiscriminator = type.GetString();
            var json = doc.RootElement.GetRawText();

            return typeDiscriminator switch
            {
                nameof(ItemCreatedEvent) => JsonSerializer.Deserialize<ItemCreatedEvent>(json, options),
                nameof(ItemEditedEvent) => JsonSerializer.Deserialize<ItemEditedEvent>(json, options),
                nameof(ItemDeletedEvent) => JsonSerializer.Deserialize<ItemDeletedEvent>(json, options),
                nameof(ItemPermanentlyDeletedEvent) => JsonSerializer.Deserialize<ItemPermanentlyDeletedEvent>(json, options),
                nameof(ItemTypeCreatedEvent) => JsonSerializer.Deserialize<ItemTypeCreatedEvent>(json, options),
                nameof(ItemTypeEditedEvent) => JsonSerializer.Deserialize<ItemTypeEditedEvent>(json, options),
                nameof(ItemTypeDeletedEvent) => JsonSerializer.Deserialize<ItemTypeDeletedEvent>(json, options),
                nameof(ItemTypePermanentlyDeletedEvent) => JsonSerializer.Deserialize<ItemTypePermanentlyDeletedEvent>(json, options),
				nameof(ItemCategoryCreatedEvent) => JsonSerializer.Deserialize<ItemCategoryCreatedEvent>(json, options),
				nameof(ItemCategoryEditedEvent) => JsonSerializer.Deserialize<ItemCategoryEditedEvent>(json, options),
				nameof(ItemCategoryDeletedEvent) => JsonSerializer.Deserialize<ItemCategoryDeletedEvent>(json, options),
				nameof(ItemCategoryPermanentlyDeletedEvent) => JsonSerializer.Deserialize<ItemCategoryPermanentlyDeletedEvent>(json, options),

				_ => throw new JsonException($"{typeDiscriminator} is not supported yet!")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}