using Items.Service.Query.Application.Interfaces;
using Items.Service.Query.Application.Queries.Items;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Domain.Interfaces;

namespace Items.Service.Query.Application.Handlers
{
    public class ItemQueryHandler : IItemQueryHandler
    {
        private readonly IItemRepository _itemRepository;

        public ItemQueryHandler(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<List<ItemEntity>> HandleAsync(FindAllItemsQuery query)
        {
            return await _itemRepository.ListAllAsync();
        }

        public async Task<List<ItemEntity>> HandleAsync(FindItemByIdQuery query)
        {
            var item = await _itemRepository.GetByIdAsync(query.Id);
            return new List<ItemEntity> { item };
        }

        public async Task<List<ItemEntity>> HandleAsync(FindItemByCodeQuery query)
        {
            var item = await _itemRepository.GetByCodeAsync(query.Code);
            return new List<ItemEntity> { item };
        }

        public async Task<List<ItemEntity>> HandleAsync(FindDeletedItemsQuery query)
        {
            return await _itemRepository.ListDeletedAsync();
        }
    }
}