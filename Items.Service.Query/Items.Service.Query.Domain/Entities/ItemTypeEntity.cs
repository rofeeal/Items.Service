using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Items.Service.Query.Domain.Entities
{
    [Table("ItemType", Schema = "dbo")]
    public class ItemTypeEntity : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ForeignName { get; set; }
        public Guid? BranchId { get; set; }
        public string? Notes { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
