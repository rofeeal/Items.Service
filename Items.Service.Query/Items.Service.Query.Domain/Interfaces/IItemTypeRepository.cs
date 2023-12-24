using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Domain.Interfaces
{
    public interface IItemTypeRepository
    {
        Task CreateAsync(ItemTypeEntity itemType);
        Task UpdateAsync(ItemTypeEntity itemType);
        Task DeleteAsync(Guid itemTypeId);
        Task DeletePermanentlyAsync(Guid itemTypeId);
        Task<ItemTypeEntity> GetByIdAsync(Guid itemTypeId);
        Task<ItemTypeEntity> GetByCodeAsync(string itemTypeCode);
        Task<List<ItemTypeEntity>> ListAllAsync();
        Task<List<ItemTypeEntity>> ListDeletedAsync();
    }
}
