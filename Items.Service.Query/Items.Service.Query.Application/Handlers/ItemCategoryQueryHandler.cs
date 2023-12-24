using Items.Service.Query.Application.Interfaces;
using Items.Service.Query.Application.Queries.ItemsCategories;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Domain.Interfaces;

namespace Items.Service.Query.Application.Handlers
{
    public class ItemCategoryQueryHandler : IItemCategoryQueryHandler
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;

        public ItemCategoryQueryHandler(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        public async Task<List<ItemCategoryEntity>> HandleAsync(FindAllItemsCategoriesQuery query)
        {
            return await _itemCategoryRepository.ListAllAsync();
        }

        public async Task<List<ItemCategoryEntity>> HandleAsync(FindItemCategoryByIdQuery query)
        {
            var itemCategory = await _itemCategoryRepository.GetByIdAsync(query.Id);
            return new List<ItemCategoryEntity> { itemCategory };
        }

        public async Task<List<ItemCategoryEntity>> HandleAsync(FindItemCategoryByCodeQuery query)
        {
            var itemCategory = await _itemCategoryRepository.GetByCodeAsync(query.Code);
            return new List<ItemCategoryEntity> { itemCategory };
        }

        public async Task<List<ItemCategoryEntity>> HandleAsync(FindDeletedItemsCategoriesQuery query)
        {
            return await _itemCategoryRepository.ListDeletedAsync();
        }
    }
}