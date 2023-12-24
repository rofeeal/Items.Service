using Items.Service.Common.DTOs;
using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Application.DTOs
{
    public class ItemLookupResponse : BaseResponse
    {
        public List<ItemEntity> Results { get; set; }
    }
}