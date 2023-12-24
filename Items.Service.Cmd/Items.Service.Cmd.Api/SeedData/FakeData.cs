using CQRS.Core.Interfaces;
using Items.Service.Cmd.Application.Commands.Items;
using Items.Service.Cmd.Application.Commands.ItemsTypes;
using Newtonsoft.Json;

namespace Items.Service.Query.Infrastructure.SeedData
{
    public static class FakeData
    {
        public static void Initialize(ICommandDispatcher commandDispatcher)
        {
            // Get the path to the JSON file in the same directory as this class
            var jsonFileName = "FakeData.json";
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), jsonFileName);

            // Check if the JSON file exists
            if (!File.Exists(jsonFilePath))
            {
                return; // JSON file doesn't exist, no faking data required
            }

            // Read fake data from JSON file
            var jsonData = File.ReadAllText(jsonFilePath);
            var fakeData = JsonConvert.DeserializeObject<FakeDataModel>(jsonData);

            foreach (var item in fakeData.Items)
            {
                commandDispatcher.SendAsync(item);
            }

            foreach (var type in fakeData.ItemTypes)
            {
                commandDispatcher.SendAsync(type);
            }
        }
    }

    public class FakeDataModel
    {
        public List<SeedItemCommand> Items { get; set; }
        public List<SeedItemTypeCommand> ItemTypes { get; set; }
    }
}
