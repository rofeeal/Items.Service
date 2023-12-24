using Items.Service.Common.DTOs;
using Items.Service.Query.Domain.Entities;

namespace Items.Service.Query.Application.DTOs
{
    public class ItemTypeLookupResponse : BaseResponse
    {
        public List<ItemTypeEntity> Results { get; set; }
    }
}