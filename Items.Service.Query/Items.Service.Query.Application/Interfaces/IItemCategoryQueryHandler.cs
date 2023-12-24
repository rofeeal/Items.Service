using Items.Service.Query.Application.Queries.ItemsCategories;
using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Application.Interfaces
{
    public interface IItemCategoryQueryHandler
    {
        Task<List<ItemCategoryEntity>> HandleAsync(FindAllItemsCategoriesQuery query);
        Task<List<ItemCategoryEntity>> HandleAsync(FindDeletedItemsCategoriesQuery query);
        Task<List<ItemCategoryEntity>> HandleAsync(FindItemCategoryByIdQuery query);
        Task<List<ItemCategoryEntity>> HandleAsync(FindItemCategoryByCodeQuery query);
    }
}
