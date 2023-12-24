using Items.Service.Common.DTOs;

namespace Items.Service.Cmd.Application.DTOs
{
    public class NewItemCategoryResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}