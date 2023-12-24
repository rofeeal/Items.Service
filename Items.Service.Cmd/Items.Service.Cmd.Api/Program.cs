using Confluent.Kafka;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Interfaces;
using CQRS.Core.Producers;
using MongoDB.Bson.Serialization;
using Items.Service.Cmd.Application.Handlers;
using Items.Service.Cmd.Application.Interfaces;
using Items.Service.Cmd.Domain.Aggregates;
using Items.Service.Cmd.Infrastructure.Config;
using Items.Service.Cmd.Infrastructure.Dispatchers;
using Items.Service.Cmd.Infrastructure.Handlers;
using Items.Service.Cmd.Infrastructure.Producers;
using Items.Service.Cmd.Infrastructure.Repositories;
using Items.Service.Cmd.Infrastructure.Stores;
using Items.Service.Common.Events;
using Items.Service.Cmd.Application.Commands.Items;
using Items.Service.Cmd.Infrastructure.Interfaces;
using Items.Service.Cmd.Application.Commands.ItemsTypes;
using Items.Service.Query.Infrastructure.SeedData;
using Items.Service.Cmd.Application.Commands.ItemsCategories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<ItemCreatedEvent>();
BsonClassMap.RegisterClassMap<ItemEditedEvent>();
BsonClassMap.RegisterClassMap<ItemDeletedEvent>();
BsonClassMap.RegisterClassMap<ItemPermanentlyDeletedEvent>();
BsonClassMap.RegisterClassMap<ItemTypeCreatedEvent>();
BsonClassMap.RegisterClassMap<ItemTypeEditedEvent>();
BsonClassMap.RegisterClassMap<ItemTypeDeletedEvent>();
BsonClassMap.RegisterClassMap<ItemTypePermanentlyDeletedEvent>();
BsonClassMap.RegisterClassMap<ItemCategoryCreatedEvent>();
BsonClassMap.RegisterClassMap<ItemCategoryEditedEvent>();
BsonClassMap.RegisterClassMap<ItemCategoryDeletedEvent>();
BsonClassMap.RegisterClassMap<ItemCategoryPermanentlyDeletedEvent>();

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (env.Equals("Development"))
{
    builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
    builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
}
else
{
    var connectionString = builder.Configuration["MONGODB_CONNECTION_STRING"];
    // Add services to the container.
    builder.Services.Configure<MongoDbConfig>(options =>
    {
        options.ConnectionString = connectionString;
        options.Database = builder.Configuration["MONGODB_DATABASE"];
        options.ItemCollection = builder.Configuration["MONGODB_ITEM_COLLECTION"];
        options.ItemTypeCollection = builder.Configuration["MONGODB_ITEM_TYPE_COLLECTION"];
        options.ItemCategoryCollection = builder.Configuration["MONGODB_ITEM_CATEGORY_COLLECTION"];
	});
    builder.Services.Configure<ProducerConfig>(options =>
    {
        options.BootstrapServers = builder.Configuration["PRODUCER_BOOTSTRAP_SERVERS"];
    });
}

builder.Services.AddScoped<IItemEventStoreRepository, ItemEventStoreRepository>();
builder.Services.AddScoped<IItemTypeEventStoreRepository, ItemTypeEventStoreRepository>();
builder.Services.AddScoped<IItemCategoryEventStoreRepository, ItemCategoryEventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore<ItemAggregate>, ItemEventStore>();
builder.Services.AddScoped<IEventStore<ItemTypeAggregate>, ItemTypeEventStore>();
builder.Services.AddScoped<IEventStore<ItemCategoryAggregate>, ItemCategoryEventStore>();

builder.Services.AddScoped<IEventSourcingHandler<ItemAggregate>, ItemEventSourcingHandler>();
builder.Services.AddScoped<IEventSourcingHandler<ItemTypeAggregate>, ItemTypeEventSourcingHandler>();
builder.Services.AddScoped<IEventSourcingHandler<ItemCategoryAggregate>, ItemCategoryEventSourcingHandler>();
builder.Services.AddScoped<IItemCommandHandler, ItemCommandHandler>();
builder.Services.AddScoped<IItemTypeCommandHandler, ItemTypeCommandHandler>();
builder.Services.AddScoped<IItemCategoryCommandHandler, ItemCategoryCommandHandler>();

// register command handler methods
var itemCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IItemCommandHandler>();
var itemTypeCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IItemTypeCommandHandler>();
var itemCategoryCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IItemCategoryCommandHandler>();

var dispatcher = new CommandDispatcher();
dispatcher.RegisterHandler<NewItemCommand>(itemCommandHandler.HandleAsync);
dispatcher.RegisterHandler<EditItemCommand>(itemCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteItemCommand>(itemCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteItemPermanentlyCommand>(itemCommandHandler.HandleAsync);
dispatcher.RegisterHandler<RestoreReadDbItemCommand>(itemCommandHandler.HandleAsync);
dispatcher.RegisterHandler<SeedItemCommand>(itemCommandHandler.HandleAsync);

dispatcher.RegisterHandler<NewItemTypeCommand>(itemTypeCommandHandler.HandleAsync);
dispatcher.RegisterHandler<EditItemTypeCommand>(itemTypeCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteItemTypeCommand>(itemTypeCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteItemTypePermanentlyCommand>(itemTypeCommandHandler.HandleAsync);
dispatcher.RegisterHandler<RestoreReadDbItemTypeCommand>(itemTypeCommandHandler.HandleAsync);
dispatcher.RegisterHandler<SeedItemTypeCommand>(itemTypeCommandHandler.HandleAsync);

dispatcher.RegisterHandler<NewItemCategoryCommand>(itemCategoryCommandHandler.HandleAsync);
dispatcher.RegisterHandler<EditItemCategoryCommand>(itemCategoryCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteItemCategoryCommand>(itemCategoryCommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteItemCategoryPermanentlyCommand>(itemCategoryCommandHandler.HandleAsync);
dispatcher.RegisterHandler<RestoreReadDbItemCategoryCommand>(itemCategoryCommandHandler.HandleAsync);
dispatcher.RegisterHandler<SeedItemCategoryCommand>(itemCategoryCommandHandler.HandleAsync);

builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

SeedData.Initialize(dispatcher);

bool useFakeData = builder.Configuration.GetValue<bool>("FAKE_DATA");
if (useFakeData)
{
    FakeData.Initialize(dispatcher);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
using (var serviceScope = app.Services.CreateScope())
{
    var eventProducer = serviceScope.ServiceProvider.GetRequiredService<IEventProducer>();

    // Call CreateKafkaTopicIfNotExists method
    Task.Run(async () =>
    {
        await eventProducer.CreateKafkaTopicIfNotExists(topic);
    }).Wait(); // Wait for the task to complete (synchronously) - ensure it's safe to do so in your context
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
