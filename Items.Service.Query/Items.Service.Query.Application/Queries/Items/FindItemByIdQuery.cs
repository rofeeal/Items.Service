using CQRS.Core.Queries;

namespace Items.Service.Query.Application.Queries.Items
{
    public class FindItemByIdQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}
