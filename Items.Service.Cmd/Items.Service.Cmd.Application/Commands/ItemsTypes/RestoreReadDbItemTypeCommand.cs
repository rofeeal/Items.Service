using CQRS.Core.Commands;

namespace Items.Service.Cmd.Application.Commands.ItemsTypes
{
    public class RestoreReadDbItemTypeCommand : BaseCommand
    {
        public string AggregateType { get; set; }
    }
}
