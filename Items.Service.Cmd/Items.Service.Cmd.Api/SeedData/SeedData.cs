using Newtonsoft.Json;
using CQRS.Core.Interfaces;
using Items.Service.Cmd.Application.Commands.Items;
using Items.Service.Cmd.Application.Commands.ItemsTypes;
using Items.Service.Cmd.Application.Commands.ItemsCategories;

namespace Items.Service.Query.Infrastructure.SeedData
{
    public static class SeedData
    {
        public static void Initialize(ICommandDispatcher commandDispatcher)
        {
            // Get the path to the JSON file in the same directory as this class
            var jsonFileName = "SeedData.json";
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), jsonFileName);

            // Check if the JSON file exists
            if (!File.Exists(jsonFilePath))
            {
                return; // JSON file doesn't exist, no seeding required
            }

            // Read seed data from JSON file
            var jsonData = File.ReadAllText(jsonFilePath);
            var seedData = JsonConvert.DeserializeObject<SeedDataModel>(jsonData);

            foreach (var item in seedData.Items)
            {
                commandDispatcher.SendAsync(item);
            }

            foreach (var type in seedData.ItemTypes)
            {
                commandDispatcher.SendAsync(type);
            }

			foreach (var category in seedData.ItemCategories)
			{
				commandDispatcher.SendAsync(category);
			}
		}
    }

    public class SeedDataModel
    {
        public List<SeedItemCommand> Items { get; set; }
        public List<SeedItemTypeCommand> ItemTypes { get; set; }
        public List<SeedItemCategoryCommand> ItemCategories { get; set; }
	}
}
