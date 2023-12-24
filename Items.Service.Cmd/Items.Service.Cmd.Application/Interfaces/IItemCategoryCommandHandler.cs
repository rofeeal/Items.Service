using Items.Service.Cmd.Application.Commands.ItemsCategories;

namespace Items.Service.Cmd.Application.Interfaces
{
    public interface IItemCategoryCommandHandler
    {
        Task HandleAsync(NewItemCategoryCommand command);
        Task HandleAsync(EditItemCategoryCommand command);
        Task HandleAsync(DeleteItemCategoryCommand command);
        Task HandleAsync(DeleteItemCategoryPermanentlyCommand command);
        Task HandleAsync(RestoreReadDbItemCategoryCommand command);
    }
}
