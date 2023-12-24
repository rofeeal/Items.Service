using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Items.Service.Query.Application.Handlers;
using Items.Service.Query.Application.Interfaces;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Domain.Interfaces;
using Items.Service.Query.Infrastructure.Consumers;
using Items.Service.Query.Infrastructure.DataAccess;
using Items.Service.Query.Infrastructure.Dispatchers;
using Items.Service.Query.Infrastructure.Handlers;
using Items.Service.Query.Infrastructure.Interfaces;
using Items.Service.Query.Infrastructure.Repositories;
using Items.Service.Query.Application.Queries.Items;
using Items.Service.Query.Application.Queries.ItemsTypes;
using Items.Service.Query.Application.Queries.ItemsCategories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Action<DbContextOptionsBuilder> configureDbContext;
configureDbContext = o => o.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("CONNECTION_STRING"));

/*var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (env.Equals("Development")){}
else
{
    configureDbContext = o => o.UseSqlServer(builder.Configuration.GetConnectionString("CONNECTION_STRING"));
}*/

builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton(new DatabaseContextFactory(configureDbContext));

// create database and tables
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemTypeRepository, ItemTypeRepository>();
builder.Services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();
builder.Services.AddScoped<IItemQueryHandler, ItemQueryHandler>();
builder.Services.AddScoped<IItemTypeQueryHandler, ItemTypeQueryHandler>();
builder.Services.AddScoped<IItemCategoryQueryHandler, ItemCategoryQueryHandler>();
builder.Services.AddScoped<IEventHandler, AllEventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

// register query handler methods
var itemQueryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IItemQueryHandler>();
var itemTypeQueryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IItemTypeQueryHandler>();
var itemCategoryQueryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IItemCategoryQueryHandler>();

var itemDispatcher = new ItemQueryDispatcher();
var itemTypeDispatcher = new ItemTypeQueryDispatcher();
var itemCategoryDispatcher = new ItemCategoryQueryDispatcher();

itemDispatcher.RegisterHandler<FindAllItemsQuery>(itemQueryHandler.HandleAsync);
itemDispatcher.RegisterHandler<FindDeletedItemsQuery>(itemQueryHandler.HandleAsync);
itemDispatcher.RegisterHandler<FindItemByIdQuery>(itemQueryHandler.HandleAsync);
itemDispatcher.RegisterHandler<FindItemByCodeQuery>(itemQueryHandler.HandleAsync);

itemTypeDispatcher.RegisterHandler<FindAllItemsTypesQuery>(itemTypeQueryHandler.HandleAsync);
itemTypeDispatcher.RegisterHandler<FindDeletedItemsTypesQuery>(itemTypeQueryHandler.HandleAsync);
itemTypeDispatcher.RegisterHandler<FindItemTypeByIdQuery>(itemTypeQueryHandler.HandleAsync);
itemTypeDispatcher.RegisterHandler<FindItemTypeByCodeQuery>(itemTypeQueryHandler.HandleAsync);

itemCategoryDispatcher.RegisterHandler<FindAllItemsCategoriesQuery>(itemCategoryQueryHandler.HandleAsync);
itemCategoryDispatcher.RegisterHandler<FindDeletedItemsCategoriesQuery>(itemCategoryQueryHandler.HandleAsync);
itemCategoryDispatcher.RegisterHandler<FindItemCategoryByIdQuery>(itemCategoryQueryHandler.HandleAsync);
itemCategoryDispatcher.RegisterHandler<FindItemCategoryByCodeQuery>(itemCategoryQueryHandler.HandleAsync);

builder.Services.AddScoped<IQueryDispatcher<ItemEntity>>(_ => itemDispatcher);
builder.Services.AddScoped<IQueryDispatcher<ItemTypeEntity>>(_ => itemTypeDispatcher);
builder.Services.AddScoped<IQueryDispatcher<ItemCategoryEntity>>(_ => itemCategoryDispatcher);

builder.Services.AddControllers();
builder.Services.AddHostedService<ConsumerHostedService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
