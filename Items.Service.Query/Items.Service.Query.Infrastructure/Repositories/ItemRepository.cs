using Microsoft.EntityFrameworkCore;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Infrastructure.DataAccess;
using Items.Service.Query.Domain.Interfaces;

namespace Items.Service.Query.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public ItemRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(ItemEntity item)
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    context.Items.Add(item);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine($"An error occurred while creating the item: {ex.Message}");
            }
        }

        public async Task DeleteAsync(Guid itemId)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var item = await GetByIdAsync(itemId);

                    if (item == null)
                    {
                        throw new Exception("Item not found.");
                    }

                    context.Items.Update(item);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception("Failed to mark the item as deleted. Please try again later.", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error occurred while marking the item as deleted.", ex);
                }
            }
        }

        public async Task DeletePermanentlyAsync(Guid itemId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();

            try
            {
                var item = await GetByIdAsync(itemId);

                if (item == null)
                {
                    throw new Exception("Item not found.");
                }

                context.Items.Remove(item);
                _ = await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to delete item. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the item.", ex);
            }
        }

        public async Task<ItemEntity> GetByCodeAsync(string itemCode)
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    return await context.Items
                        .Include(t => t.Type).Include(t => t.Category) 
                        .Where(x => x.Code == itemCode)
                        .FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the item with code {itemCode}.", ex);
            }
        }

        public async Task<ItemEntity> GetByIdAsync(Guid itemId)
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    return await context.Items.Include(t => t.Type).Include(t => t.Category).FirstOrDefaultAsync(x => x.Id == itemId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the undeleted item list.", ex);
            }
        }

        public async Task<List<ItemEntity>> ListAllAsync()
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    return await context.Items
                        .Include(t => t.Type).Include(t => t.Category)
                        .Where(p => !p.IsDeleted) 
                        .AsNoTracking()
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the undeleted item list.", ex);
            }
        }

        public async Task<List<ItemEntity>> ListDeletedAsync()
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    return await context.Items
                        .Include(t => t.Type).Include(t => t.Category)
                        .Where(p => p.IsDeleted) 
                        .AsNoTracking()
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the deleted item list.", ex);
            }
        }

        public async Task UpdateAsync(ItemEntity item)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();

            try
            {
                context.Items.Update(item);

                _ = await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to update item. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the item.", ex);
            }
        }
    }
}
