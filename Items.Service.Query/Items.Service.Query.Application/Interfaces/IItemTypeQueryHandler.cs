using Items.Service.Query.Application.Queries.ItemsTypes;
using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Application.Interfaces
{
    public interface IItemTypeQueryHandler
    {
        Task<List<ItemTypeEntity>> HandleAsync(FindAllItemsTypesQuery query);
        Task<List<ItemTypeEntity>> HandleAsync(FindDeletedItemsTypesQuery query);
        Task<List<ItemTypeEntity>> HandleAsync(FindItemTypeByIdQuery query);
        Task<List<ItemTypeEntity>> HandleAsync(FindItemTypeByCodeQuery query);
    }
}
