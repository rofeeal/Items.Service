using Items.Service.Query.Application.Interfaces;
using Items.Service.Query.Application.Queries.Items;
using Items.Service.Query.Application.Queries.ItemsTypes;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Domain.Interfaces;

namespace Items.Service.Query.Application.Handlers
{
    public class ItemTypeQueryHandler : IItemTypeQueryHandler
    {
        private readonly IItemTypeRepository _itemTypeRepository;

        public ItemTypeQueryHandler(IItemTypeRepository itemTypeRepository)
        {
            _itemTypeRepository = itemTypeRepository;
        }

        public async Task<List<ItemTypeEntity>> HandleAsync(FindAllItemsTypesQuery query)
        {
            return await _itemTypeRepository.ListAllAsync();
        }

        public async Task<List<ItemTypeEntity>> HandleAsync(FindItemTypeByIdQuery query)
        {
            var itemType = await _itemTypeRepository.GetByIdAsync(query.Id);
            return new List<ItemTypeEntity> { itemType };
        }

        public async Task<List<ItemTypeEntity>> HandleAsync(FindItemTypeByCodeQuery query)
        {
            var itemType = await _itemTypeRepository.GetByCodeAsync(query.Code);
            return new List<ItemTypeEntity> { itemType };
        }

        public async Task<List<ItemTypeEntity>> HandleAsync(FindDeletedItemsTypesQuery query)
        {
            return await _itemTypeRepository.ListDeletedAsync();
        }
    }
}