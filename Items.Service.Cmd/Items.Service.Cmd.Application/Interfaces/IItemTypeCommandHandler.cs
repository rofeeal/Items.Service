using Items.Service.Cmd.Application.Commands.ItemsTypes;

namespace Items.Service.Cmd.Application.Interfaces
{
    public interface IItemTypeCommandHandler
    {
        Task HandleAsync(NewItemTypeCommand command);
        Task HandleAsync(EditItemTypeCommand command);
        Task HandleAsync(DeleteItemTypeCommand command);
        Task HandleAsync(DeleteItemTypePermanentlyCommand command);
        Task HandleAsync(RestoreReadDbItemTypeCommand command);
    }
}
