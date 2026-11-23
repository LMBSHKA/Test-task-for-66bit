namespace Application.Interfaces.RepoIntrefaces
{
    public interface IWarehouseUnitOfWork
    {
        IEquipmentTypeRepo EquipmentTypes { get; }
        IEquipmentItemRepo EquipmentItems { get; }
        IManufacturerRepo Manufacturers { get; }
        ICountryRepo Countries { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
