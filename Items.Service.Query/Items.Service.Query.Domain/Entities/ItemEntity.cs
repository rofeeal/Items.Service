using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Items.Service.Query.Domain.Entities
{
    [Table("Item", Schema = "dbo")]
    public class ItemEntity : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
		public string? Code { get; set; }
		public string Name { get; set; }
		public string ForeignName { get; set; }
		public Guid? TypeId { get; set; }
		public virtual ItemTypeEntity? Type { get; set; }
		public Guid? CategoryId { get; set; }
		public virtual ItemCategoryEntity? Category { get; set; }
		public Guid? UnitId { get; set; }
		public Guid? SupplierId { get; set; }
		public decimal Price { get; set; }
		public decimal Cost { get; set; }
		public Guid? TaxCodeTypeId { get; set; }
		public string? TaxCode { get; set; }
		public bool IsForPurchase { get; set; }
		public bool IsForSale { get; set; }
		public string? Notes { get; set; }
		public bool IsActive { get; set; }
	}
}
