using CQRS.Core.Queries;

namespace Items.Service.Query.Application.Queries.Items
{
    public class FindItemByCodeQuery : BaseQuery
    {
        public string Code { get; set; }
    }
}
