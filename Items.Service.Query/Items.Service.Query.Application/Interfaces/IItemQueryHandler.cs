using Items.Service.Query.Application.Queries.Items;
using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Application.Interfaces
{
    public interface IItemQueryHandler
    {
        Task<List<ItemEntity>> HandleAsync(FindAllItemsQuery query);
        Task<List<ItemEntity>> HandleAsync(FindDeletedItemsQuery query);
        Task<List<ItemEntity>> HandleAsync(FindItemByIdQuery query);
        Task<List<ItemEntity>> HandleAsync(FindItemByCodeQuery query);
    }
}
