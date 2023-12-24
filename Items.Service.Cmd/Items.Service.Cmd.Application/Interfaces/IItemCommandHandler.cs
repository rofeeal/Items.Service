using Items.Service.Cmd.Application.Commands.Items;

namespace Items.Service.Cmd.Application.Interfaces
{
    public interface IItemCommandHandler
    {
        Task HandleAsync(NewItemCommand command);
        Task HandleAsync(EditItemCommand command);
        Task HandleAsync(DeleteItemCommand command);
        Task HandleAsync(DeleteItemPermanentlyCommand command);
        Task HandleAsync(RestoreReadDbItemCommand command);
    }
}
