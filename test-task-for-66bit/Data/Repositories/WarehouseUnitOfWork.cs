using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Microsoft.EntityFrameworkCore;
using Models.Interfaces;
using Npgsql;

namespace Data.Repositories
{
    public class WarehouseUnitOfWork : IWarehouseUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public WarehouseUnitOfWork(
            AppDbContext dbContext,
            IEquipmentTypeRepo equipmentTypes,
            IManufacturerRepo manufacturers,
            IEquipmentItemRepo equipmentItems,
            ICountryRepo country)
        {
            _dbContext = dbContext;
            EquipmentTypes = equipmentTypes;
            Manufacturers = manufacturers;
            EquipmentItems = equipmentItems;
            Countries = country;
        }

        public IEquipmentTypeRepo EquipmentTypes { get; }
        public IManufacturerRepo Manufacturers { get; }
        public IEquipmentItemRepo EquipmentItems { get; }
        public ICountryRepo Countries { get; }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _dbContext.SaveChangesAsync(ct);

        // Сделал такой метод, если база запрещает удаление из-за связанных позиций, возвращаем false,
        // чтобы API мог ответить 409 Conflict.
        public async Task<bool> TrySaveCatalogDeletionAsync(ICatalogEntry entry, CancellationToken ct)
        {
            try
            {
                await SaveChangesAsync(ct);
                return true;
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException
            { SqlState: PostgresErrorCodes.ForeignKeyViolation })
            {
                _dbContext.Entry(entry).State = EntityState.Unchanged;
                return false;
            }
        }
    }
}
