using CQRS.Core.Queries;

namespace Items.Service.Query.Application.Queries.ItemsTypes
{
    public class FindItemTypeByIdQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}
