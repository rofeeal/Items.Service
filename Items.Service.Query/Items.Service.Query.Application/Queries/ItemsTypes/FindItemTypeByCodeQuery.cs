using CQRS.Core.Queries;

namespace Items.Service.Query.Application.Queries.ItemsTypes
{
    public class FindItemTypeByCodeQuery : BaseQuery
    {
        public string Code { get; set; }
    }
}
