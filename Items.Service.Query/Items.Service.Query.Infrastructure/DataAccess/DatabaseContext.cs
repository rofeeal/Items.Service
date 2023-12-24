using Items.Service.Query.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Items.Service.Query.Infrastructure.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<ItemTypeEntity> ItemTypes { get; set; }
        public DbSet<ItemCategoryEntity> ItemCategories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemEntity>()
                .HasOne(c => c.Type)
                .WithMany()
                .HasForeignKey(c => c.TypeId);

			modelBuilder.Entity<ItemEntity>()
				.HasOne(c => c.Category)
				.WithMany()
				.HasForeignKey(c => c.CategoryId);
		}
    }
}