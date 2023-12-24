namespace Items.Service.Cmd.Infrastructure.Config
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string ItemCollection { get; set; }
		public string ItemTypeCollection { get; set; }
		public string ItemCategoryCollection { get; set; }
	}
}
