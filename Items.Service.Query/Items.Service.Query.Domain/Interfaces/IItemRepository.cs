
using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Domain.Interfaces
{
    public interface IItemRepository
    {
        Task CreateAsync(ItemEntity item);
        Task UpdateAsync(ItemEntity item);
        Task DeleteAsync(Guid itemId);
        Task DeletePermanentlyAsync(Guid itemId);
        Task<ItemEntity> GetByIdAsync(Guid itemId);
        Task<ItemEntity> GetByCodeAsync(string itemCode);
        Task<List<ItemEntity>> ListAllAsync();
        Task<List<ItemEntity>> ListDeletedAsync();
    }
}
