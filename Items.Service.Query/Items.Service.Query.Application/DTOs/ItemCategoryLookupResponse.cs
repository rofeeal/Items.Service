using Items.Service.Common.DTOs;
using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Application.DTOs
{
    public class ItemCategoryLookupResponse : BaseResponse
    {
        public List<ItemCategoryEntity> Results { get; set; }
    }
}