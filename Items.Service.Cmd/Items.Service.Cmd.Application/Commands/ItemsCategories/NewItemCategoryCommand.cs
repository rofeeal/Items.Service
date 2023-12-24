using CQRS.Core.Commands;

namespace Items.Service.Cmd.Application.Commands.ItemsCategories
{
    public class NewItemCategoryCommand : BaseCommand
    {
        
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ForeignName { get; set; }
        public Guid? BranchId { get; set; }
        public string? Notes { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
