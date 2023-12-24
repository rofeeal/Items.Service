using CQRS.Core.Queries;

namespace Items.Service.Query.Application.Queries.ItemsCategories
{
    public class FindItemCategoryByCodeQuery : BaseQuery
    {
        public string Code { get; set; }
    }
}
