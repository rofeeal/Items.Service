
using CQRS.Core.Infrastructure;
using CQRS.Core.Queries;
using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Infrastructure.Dispatchers
{
    public class ItemQueryDispatcher : IQueryDispatcher<ItemEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<ItemEntity>>>> _handlers = new();

        public void RegisterHandler<TQuery>(Func<TQuery, Task<List<ItemEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new IndexOutOfRangeException("You cannot register the same query handler twice!");
            }

            _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
        }

        public async Task<List<ItemEntity>> SendAsync(BaseQuery query)
        {
            if (_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<ItemEntity>>> handler))
            {
                return await handler(query);
            }

            throw new ArgumentNullException(nameof(handler), "No query handler was registered!");
        }
    }
}