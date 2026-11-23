using Application.Interfaces.RepoIntrefaces;
using Data.DB;

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
    }
}
