using CQRS.Core.Handlers;
using Items.Service.Cmd.Application.Commands.ItemsTypes;
using Items.Service.Cmd.Application.Interfaces;
using Items.Service.Cmd.Domain.Aggregates;

namespace Items.Service.Cmd.Application.Handlers
{
    public class ItemTypeCommandHandler : IItemTypeCommandHandler
    {
        private readonly IEventSourcingHandler<ItemTypeAggregate> _eventSourcingHandler;

        public ItemTypeCommandHandler(IEventSourcingHandler<ItemTypeAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async Task HandleAsync(NewItemTypeCommand command)
        {
            var aggregate = new ItemTypeAggregate(
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

        public async Task HandleAsync(EditItemTypeCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditItemTypeAggregate(
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

        public async Task HandleAsync(DeleteItemTypeCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);

            aggregate.DeleteItemType(command.Id);
            aggregate.SetDeletedInfo(Guid.Empty);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeleteItemTypePermanentlyCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeleteItemTypePermanently(command.Id);

            aggregate.SetDeletedInfo(Guid.Empty);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RestoreReadDbItemTypeCommand command)
        {
            await _eventSourcingHandler.RepublishEventsAsync(command.AggregateType);
        }

        public async Task HandleAsync(SeedItemTypeCommand command)
        {
            var exists = await _eventSourcingHandler.GetByIdAsync(command.Id);
            if (exists == null || exists.Id == null)
            {
                var aggregate = new ItemTypeAggregate(
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