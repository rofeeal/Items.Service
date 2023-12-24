using Items.Service.Common.Events;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Domain.Interfaces;
using Items.Service.Query.Infrastructure.Interfaces;

namespace Items.Service.Query.Infrastructure.Handlers
{
    public class AllEventHandler : IEventHandler
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IItemCategoryRepository _itemCategoryRepository;

		public AllEventHandler(IItemRepository itemRepository, IItemTypeRepository itemTypeRepository, IItemCategoryRepository itemCategoryRepository)
		{
			_itemRepository = itemRepository;
			_itemTypeRepository = itemTypeRepository;
			_itemCategoryRepository = itemCategoryRepository;
		}

		public async Task On(ItemCreatedEvent @event)
        {
            var item = new ItemEntity
            {
				Id = @event.Id,
				Code = @event.Code,
				Name = @event.Name,
				ForeignName = @event.ForeignName,
				TypeId = @event.TypeId,
				CategoryId = @event.CategoryId,
				UnitId = @event.UnitId,
				SupplierId = @event.SupplierId,
				TaxCodeTypeId = @event.TaxCodeTypeId,
				TaxCode = @event.TaxCode,
				Notes = @event.Notes,
				Price = @event.Price,
				Cost = @event.Cost,
				IsForSale = @event.IsForSale,
				IsForPurchase = @event.IsForPurchase,
				IsActive = @event.IsActive
			};

            await _itemRepository.CreateAsync(item);
        }

        public async Task On(ItemEditedEvent @event)
        {
            var item = await _itemRepository.GetByIdAsync(@event.Id);

            if (item == null) return;

			item.Code = @event.Code;
			item.Name = @event.Name;
			item.ForeignName = @event.ForeignName;
			item.TypeId = @event.TypeId;
			item.CategoryId = @event.CategoryId;
			item.UnitId = @event.UnitId;
			item.SupplierId = @event.SupplierId;
			item.TaxCodeTypeId = @event.TaxCodeTypeId;
			item.TaxCode = @event.TaxCode;
			item.Notes = @event.Notes;
			item.Price = @event.Price;
			item.Cost = @event.Cost;
			item.IsForSale = @event.IsForSale;
			item.IsForPurchase = @event.IsForPurchase;
			item.IsActive = @event.IsActive;

			await _itemRepository.UpdateAsync(item);
        }

        public async Task On(ItemDeletedEvent @event)
        {
            var item = await _itemRepository.GetByIdAsync(@event.Id);

            if (item == null) return;

            await _itemRepository.DeleteAsync(item.Id);
        }

        public async Task On(ItemPermanentlyDeletedEvent @event)
        {
            var item = await _itemRepository.GetByIdAsync(@event.Id);

            if (item == null) return;

            await _itemRepository.DeletePermanentlyAsync(item.Id);
        }

        //ITEMS_TYPES_EVENTS
        public async Task On(ItemTypeCreatedEvent @event)
        {
            var itemType = new ItemTypeEntity
            {
                Id = @event.Id,
                Code = @event.Code,
                Name = @event.Name,
                ForeignName = @event.ForeignName,
                BranchId = @event.BranchId,
                Notes = @event.Notes,
                IsActive = @event.IsActive
            };

            await _itemTypeRepository.CreateAsync(itemType);
        }

        public async Task On(ItemTypeEditedEvent @event)
        {
            var itemType = await _itemTypeRepository.GetByIdAsync(@event.Id);

            if (itemType == null) return;

            itemType.Code = @event.Code;
            itemType.Name = @event.Name;
            itemType.ForeignName = @event.ForeignName;
            itemType.BranchId = @event.BranchId;
            itemType.Notes = @event.Notes;
            itemType.IsActive = @event.IsActive;

            await _itemTypeRepository.UpdateAsync(itemType);
        }

        public async Task On(ItemTypeDeletedEvent @event)
        {
            var itemType = await _itemTypeRepository.GetByIdAsync(@event.Id);

            if (itemType == null) return;

            await _itemTypeRepository.DeleteAsync(itemType.Id);
        }

        public async Task On(ItemTypePermanentlyDeletedEvent @event)
        {
            var itemType = await _itemTypeRepository.GetByIdAsync(@event.Id);

            if (itemType == null) return;

            await _itemTypeRepository.DeletePermanentlyAsync(itemType.Id);
        }

		//ITEMS_CATEGORIES_EVENTS
		public async Task On(ItemCategoryCreatedEvent @event)
		{
			var itemCategory = new ItemCategoryEntity
			{
				Id = @event.Id,
				Code = @event.Code,
				Name = @event.Name,
				ForeignName = @event.ForeignName,
				BranchId = @event.BranchId,
				Notes = @event.Notes,
				IsActive = @event.IsActive
			};

			await _itemCategoryRepository.CreateAsync(itemCategory);
		}

		public async Task On(ItemCategoryEditedEvent @event)
		{
			var itemCategory = await _itemCategoryRepository.GetByIdAsync(@event.Id);

			if (itemCategory == null) return;

			itemCategory.Code = @event.Code;
			itemCategory.Name = @event.Name;
			itemCategory.ForeignName = @event.ForeignName;
			itemCategory.BranchId = @event.BranchId;
			itemCategory.Notes = @event.Notes;
			itemCategory.IsActive = @event.IsActive;

			await _itemCategoryRepository.UpdateAsync(itemCategory);
		}

		public async Task On(ItemCategoryDeletedEvent @event)
		{
			var itemCategory = await _itemCategoryRepository.GetByIdAsync(@event.Id);

			if (itemCategory == null) return;

			await _itemCategoryRepository.DeleteAsync(itemCategory.Id);
		}

		public async Task On(ItemCategoryPermanentlyDeletedEvent @event)
		{
			var itemCategory = await _itemCategoryRepository.GetByIdAsync(@event.Id);

			if (itemCategory == null) return;

			await _itemCategoryRepository.DeletePermanentlyAsync(itemCategory.Id);
		}
	}
}
