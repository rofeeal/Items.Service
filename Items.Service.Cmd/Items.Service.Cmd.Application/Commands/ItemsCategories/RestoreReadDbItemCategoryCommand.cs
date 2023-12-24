using CQRS.Core.Commands;

namespace Items.Service.Cmd.Application.Commands.ItemsCategories
{
    public class RestoreReadDbItemCategoryCommand : BaseCommand
    {
        public string AggregateCategory { get; set; }
    }
}
