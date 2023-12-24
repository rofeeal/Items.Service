using Microsoft.EntityFrameworkCore;
using Items.Service.Query.Domain.Entities;
using Items.Service.Query.Infrastructure.DataAccess;
using Items.Service.Query.Domain.Interfaces;

namespace Items.Service.Query.Infrastructure.Repositories
{
    public class ItemTypeRepository : IItemTypeRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public ItemTypeRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(ItemTypeEntity itemType)
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    context.ItemTypes.Add(itemType);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine($"An error occurred while creating the item type: {ex.Message}");
            }
        }

        public async Task DeleteAsync(Guid itemTypeId)
        {
            using (DatabaseContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var itemType = await GetByIdAsync(itemTypeId);

                    if (itemType == null)
                    {
                        throw new Exception("Item Type not found.");
                    }

                    context.ItemTypes.Update(itemType);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception("Failed to mark the item type as deleted. Please try again later.", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error occurred while marking the item type as deleted.", ex);
                }
            }
        }

        public async Task DeletePermanentlyAsync(Guid itemTypeId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();

            try
            {
                var itemType = await GetByIdAsync(itemTypeId);

                if (itemType == null)
                {
                    throw new Exception("Item type not found.");
                }

                context.ItemTypes.Remove(itemType);
                _ = await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to delete item type. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the item type.", ex);
            }
        }

        public async Task<ItemTypeEntity> GetByCodeAsync(string itemTypeCode)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.ItemTypes
                    .Where(x => x.Code == itemTypeCode)
                    .FirstOrDefaultAsync();
        }

        public async Task<ItemTypeEntity> GetByIdAsync(Guid itemTypeId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.ItemTypes.FirstOrDefaultAsync(x => x.Id == itemTypeId);
        }

        public async Task<List<ItemTypeEntity>> ListAllAsync()
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    return await context.ItemTypes
                        .Where(p => !p.IsDeleted) 
                        .AsNoTracking()
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the undeleted item type list.", ex);
            }
        }

        public async Task<List<ItemTypeEntity>> ListDeletedAsync()
        {
            try
            {
                using (DatabaseContext context = _contextFactory.CreateDbContext())
                {
                    return await context.ItemTypes
                        .Where(p => p.IsDeleted) 
                        .AsNoTracking()
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the deleted item type list.", ex);
            }
        }

        public async Task UpdateAsync(ItemTypeEntity itemType)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();

            try
            {
                context.ItemTypes.Update(itemType);

                _ = await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to update item type. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the item type.", ex);
            }
        }
    }
}
