using Microsoft.EntityFrameworkCore;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Infrastructure.DataAccess;
using Items.Service.Query.Domain.Interfaces;

namespace Items.Service.Query.Infrastructure.Repositories
{
    public class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public ItemCategoryRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(ItemCategoryEntity itemCategory)
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    context.ItemCategories.Add(itemCategory);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine($"An error occurred while creating the item category: {ex.Message}");
            }
        }

        public async Task DeleteAsync(Guid itemCategoryId)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var itemCategory = await GetByIdAsync(itemCategoryId);

                    if (itemCategory == null)
                    {
                        throw new Exception("Item Category not found.");
                    }

                    context.ItemCategories.Update(itemCategory);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception("Failed to mark the item category as deleted. Please try again later.", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error occurred while marking the item category as deleted.", ex);
                }
            }
        }

        public async Task DeletePermanentlyAsync(Guid itemCategoryId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();

            try
            {
                var itemCategory = await GetByIdAsync(itemCategoryId);

                if (itemCategory == null)
                {
                    throw new Exception("Item category not found.");
                }

                context.ItemCategories.Remove(itemCategory);
                _ = await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to delete item category. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the item category.", ex);
            }
        }

        public async Task<ItemCategoryEntity> GetByCodeAsync(string itemCategoryCode)
        {
			try
			{

				using DatabaseContext context = _contextFactory.CreateDbContext();
                return await context.ItemCategories
                        .Where(x => x.Code == itemCategoryCode)
						.AsNoTracking()
						.FirstOrDefaultAsync();
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while retrieving the undeleted item category list.", ex);
			}
		}

        public async Task<ItemCategoryEntity> GetByIdAsync(Guid itemCategoryId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.ItemCategories.FirstOrDefaultAsync(x => x.Id == itemCategoryId);
        }

        public async Task<List<ItemCategoryEntity>> ListAllAsync()
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    return await context.ItemCategories
                        .Where(p => !p.IsDeleted) 
                        .AsNoTracking()
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the undeleted item category list.", ex);
            }
        }

        public async Task<List<ItemCategoryEntity>> ListDeletedAsync()
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    return await context.ItemCategories
                        .Where(p => p.IsDeleted) 
                        .AsNoTracking()
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the deleted item category list.", ex);
            }
        }

        public async Task UpdateAsync(ItemCategoryEntity itemCategory)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();

            try
            {
                context.ItemCategories.Update(itemCategory);

                _ = await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to update item category. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the item category.", ex);
            }
        }
    }
}
