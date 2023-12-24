using CQRS.Core.Handlers;
using Items.Service.Cmd.Application.Commands.ItemsCategories;
using Items.Service.Cmd.Application.Interfaces;
using Items.Service.Cmd.Domain.Aggregates;

namespace Items.Service.Cmd.Application.Handlers
{
    public class ItemCategoryCommandHandler : IItemCategoryCommandHandler
    {
        private readonly IEventSourcingHandler<ItemCategoryAggregate> _eventSourcingHandler;

        public ItemCategoryCommandHandler(IEventSourcingHandler<ItemCategoryAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task HandleAsync(NewItemCategoryCommand command)
        {
            var aggregate = new ItemCategoryAggregate(
                id: command.Id,
                code: command.Code,
                name: command.Name,
                foreignName: command.ForeignName,
                branchId: command.BranchId,
                notes: command.Notes,
                isActive: command.IsActive
            );

            aggregate.SetCreatedInfo(Guid.Empty);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditItemCategoryCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditItemCategoryAggregate(
                id : command.Id,
                code: command.Code,
                name: command.Name,
                foreignName: command.ForeignName,
                branchId: command.BranchId,
                notes: command.Notes,
                isActive: command.IsActive
            );
            aggregate.SetModifiedInfo(Guid.Empty);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeleteItemCategoryCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);

            aggregate.DeleteItemCategory(command.Id);
            aggregate.SetDeletedInfo(Guid.Empty);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeleteItemCategoryPermanentlyCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeleteItemCategoryPermanently(command.Id);

            aggregate.SetDeletedInfo(Guid.Empty);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RestoreReadDbItemCategoryCommand command)
        {
            await _eventSourcingHandler.RepublishEventsAsync(command.AggregateCategory);
        }

        public async Task HandleAsync(SeedItemCategoryCommand command)
        {
            var exists = await _eventSourcingHandler.GetByIdAsync(command.Id);
            if (exists == null || exists.Id == null)
            {
                var aggregate = new ItemCategoryAggregate(
                    id: command.Id,
                    code: command.Code,
                    name: command.Name,
                    foreignName: command.ForeignName,
                    branchId: command.BranchId,
                    notes: command.Notes,
                    isActive: command.IsActive
                );

                aggregate.SetCreatedInfo(Guid.Empty);
                await _eventSourcingHandler.SaveAsync(aggregate);
            }
        }
    }
}