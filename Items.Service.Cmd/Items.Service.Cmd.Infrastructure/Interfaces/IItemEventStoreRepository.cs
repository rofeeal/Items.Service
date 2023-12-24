using CQRS.Core.Domain;

namespace Items.Service.Cmd.Infrastructure.Interfaces
{
    public interface IItemEventStoreRepository : IEventStoreRepository
    {
    }
}
