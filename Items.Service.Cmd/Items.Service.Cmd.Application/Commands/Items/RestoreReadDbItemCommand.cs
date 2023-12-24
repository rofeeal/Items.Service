using CQRS.Core.Commands;

namespace Items.Service.Cmd.Application.Commands.Items
{
    public class RestoreReadDbItemCommand : BaseCommand
    {
        public string AggregateType { get; set; }
    }
}
