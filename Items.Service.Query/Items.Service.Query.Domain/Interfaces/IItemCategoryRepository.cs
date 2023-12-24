using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Domain.Interfaces
{
    public interface IItemCategoryRepository
    {
        Task CreateAsync(ItemCategoryEntity itemCategory);
        Task UpdateAsync(ItemCategoryEntity itemCategory);
        Task DeleteAsync(Guid itemCategoryId);
        Task DeletePermanentlyAsync(Guid itemCategoryId);
        Task<ItemCategoryEntity> GetByIdAsync(Guid itemCategoryId);
        Task<ItemCategoryEntity> GetByCodeAsync(string itemCategoryCode);
        Task<List<ItemCategoryEntity>> ListAllAsync();
        Task<List<ItemCategoryEntity>> ListDeletedAsync();
    }
}
