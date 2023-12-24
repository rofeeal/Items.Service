using CQRS.Core.Handlers;
using Items.Service.Cmd.Application.Commands.Items;
using Items.Service.Cmd.Application.Interfaces;
using Items.Service.Cmd.Domain.Aggregates;
using System.Xml.Linq;

namespace Items.Service.Cmd.Application.Handlers
{
    public class ItemCommandHandler : IItemCommandHandler
    {
        private readonly IEventSourcingHandler<ItemAggregate> _itemEventSourcingHandler;
        private readonly IEventSourcingHandler<ItemTypeAggregate> _itemTypeEventSourcingHandler;
        private readonly IEventSourcingHandler<ItemCategoryAggregate> _itemCategoryEventSourcingHandler;

		public ItemCommandHandler(IEventSourcingHandler<ItemAggregate> itemEventSourcingHandler, IEventSourcingHandler<ItemTypeAggregate> itemTypeEventSourcingHandler, IEventSourcingHandler<ItemCategoryAggregate> itemCategoryEventSourcingHandler)
        {
            _itemEventSourcingHandler = itemEventSourcingHandler;
            _itemTypeEventSourcingHandler = itemTypeEventSourcingHandler;
            _itemCategoryEventSourcingHandler = itemCategoryEventSourcingHandler;
        }

        public async Task HandleAsync(NewItemCommand command)
        {
            bool isValidType = await ValidateItemTypeIdAsync(command.TypeId);
            bool isValidCategory = await ValidateItemCategoryIdAsync(command.CategoryId);

			if (!isValidType || !isValidCategory)
			{
				var errors = new List<string>();

				if (!isValidType)
				{
					errors.Add("Invalid item type ID.");
				}

				if (!isValidCategory)
				{
					errors.Add("Invalid item category ID.");
				}

				throw new ArgumentException(string.Join(Environment.NewLine, errors));
			}

			var aggregate = new ItemAggregate(
                id: command.Id,
				code : command.Code,
				name : command.Name,
				foreignName : command.ForeignName,
				typeId : command.TypeId,
				categoryId : command.CategoryId,
				unitId : command.UnitId,
				supplierId : command.SupplierId,
				taxCodeTypeId : command.TaxCodeTypeId,
				taxCode : command.TaxCode,
				notes : command.Notes,
				price : command.Price,
				cost : command.Cost,
				isForSale : command.IsForSale,
				isForPurchase : command.IsForPurchase,
				isActive : command.IsActive
            );

            aggregate.SetCreatedInfo(Guid.Empty);
            await _itemEventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditItemCommand command)
        {
            bool isValid = await ValidateItemTypeIdAsync(command.TypeId);

            if (!isValid)
            {
                throw new InvalidOperationException("Item type ID does not exist");
            }

            var aggregate = await _itemEventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.EditItemAggregate(
				id: command.Id,
				code: command.Code,
				name: command.Name,
				foreignName: command.ForeignName,
				typeId: command.TypeId,
				categoryId: command.CategoryId,
				unitId: command.UnitId,
				supplierId: command.SupplierId,
				taxCodeTypeId: command.TaxCodeTypeId,
				taxCode: command.TaxCode,
				notes: command.Notes,
				price: command.Price,
				cost: command.Cost,
				isForSale: command.IsForSale,
				isForPurchase: command.IsForPurchase,
				isActive: command.IsActive
			);
            aggregate.SetModifiedInfo(Guid.Empty);
            await _itemEventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeleteItemCommand command)
        {
            var aggregate = await _itemEventSourcingHandler.GetByIdAsync(command.Id);

            aggregate.DeleteItem(command.Id);
            aggregate.SetDeletedInfo(Guid.Empty);

            await _itemEventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeleteItemPermanentlyCommand command)
        {
            var aggregate = await _itemEventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeleteItemPermanently(command.Id);

            aggregate.SetDeletedInfo(Guid.Empty);
            await _itemEventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RestoreReadDbItemCommand command)
        {
            await _itemEventSourcingHandler.RepublishEventsAsync(command.AggregateType);
        }

        private async Task<bool> ValidateItemTypeIdAsync(Guid? typeId)
        {
            if (typeId == null)
            {
                return true;
            }

            try
            {
                var getTypeId = await _itemTypeEventSourcingHandler.GetByIdAsync((Guid)typeId);
                return getTypeId != null;
            }
            catch 
            {
                return false;
            }
        }

		private async Task<bool> ValidateItemCategoryIdAsync(Guid? categoryId)
		{
			if (categoryId == null)
			{
				return true;
			}

			try
			{
				var getCategoryIdId = await _itemCategoryEventSourcingHandler.GetByIdAsync((Guid)categoryId);
				return getCategoryIdId != null;
			}
			catch
			{
				return false;
			}
		}
	}
}