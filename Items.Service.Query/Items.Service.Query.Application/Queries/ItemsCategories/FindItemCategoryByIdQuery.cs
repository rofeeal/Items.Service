using CQRS.Core.Queries;

namespace Items.Service.Query.Application.Queries.ItemsCategories
{
    public class FindItemCategoryByIdQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}
